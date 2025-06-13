using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TaskStatus = Domain.Models.TaskStatus;

namespace Application.Services;

public class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramBotService> _logger;
    private readonly Dictionary<UpdateType, Func<Update, Task>> _handlers;
    private readonly Dictionary<string, Func<Message, Task>> _messageHandlers;
    private readonly ITelegramGroupRepository _groupRepository;
    private readonly ITaskBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;

    public TelegramBotService(
        ITelegramBotClient botClient, 
        ILogger<TelegramBotService> logger, 
        ITelegramGroupRepository groupRepository,
        ITaskBoardRepository boardRepository,
        IUserRepository userRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _groupRepository = groupRepository;
        _boardRepository = boardRepository;
        _userRepository = userRepository;
        _handlers = new Dictionary<UpdateType, Func<Update, Task>>
        {
            { UpdateType.Message, HandleCommandAsync}
        };
        _messageHandlers = new Dictionary<string, Func<Message, Task>>
        {
            { "/start@marat_task_master_bot", HandleStartAsync },
            { "/link@marat_task_master_bot", HandleLinkAsync },
            { "/board@marat_task_master_bot", HandleBoardInfoAsync },
            { "/rating@marat_task_master_bot", HandleRatingAsync },
            { "/busy@marat_task_master_bot", HandleBusyUsersAsync },
            { "/help@marat_task_master_bot", HandleHelpAsync }
        };
    }

    public async Task HandleUpdateAsync(Update update)
    {
        _logger.LogInformation("Получено обновление типа {UpdateType}", update.Type);
        await _handlers[update.Type](update);
    }

    private async Task HandleCommandAsync(Update update)
    {
        if (update.Type != UpdateType.Message) 
            return;
        var message = update.Message;
        if (message == null)
            return;
        var command = message.Text?.Split(' ')[0];
        if (command == null)
            return;
            
        _logger.LogInformation("Получена команда {Command} от пользователя {UserId}", 
            command, message.From?.Id);
            
        if (_messageHandlers.TryGetValue(command, out var handler))
        {
            await handler(message);
        }
        else if (message.Chat.Id == message.From!.Id)
        {
            _logger.LogWarning("Получена неизвестная команда {Command} от пользователя {UserId}", 
                command, message.From.Id);
            await _botClient.SendMessage(
                chatId: message.Chat.Id, 
                text: "Команда не найдена. Используйте /help для просмотра списка команд");
        }
    }

    private async Task HandleStartAsync(Message message)
    {
        _logger.LogInformation("Обработка команды /start от пользователя {UserId}", message.From?.Id);
        
        if (message.Chat.Id == message.From!.Id)
        {
            await _botClient.SendMessage(
                chatId: message.Chat.Id, 
                text: "Привет! Для начала добавь меня в свою группу, а затем слинкуй меня с доской через команду /link");
            return;
        }
        
        await _botClient.SendMessage(
            chatId: message.Chat.Id, 
            text: "Привет! Я бот для управления задачами. Чтобы начать работу, используйте команду /link и укажите ссылку на доску через пробел");
    }
    
    private async Task HandleLinkAsync(Message message)
    {
        _logger.LogInformation("Обработка команды /link от пользователя {UserId}", message.From?.Id);
        
        if (message.Chat.Id == message.From!.Id)
        {
            _logger.LogWarning("Попытка линковки в личном чате от пользователя {UserId}", message.From.Id);
            await _botClient.SendMessage(
                chatId: message.Chat.Id, 
                text: "Линковать можно только в группе");
            return;
        }

        var parts = message.Text!.Split(' ');
        if (parts.Length != 2)
        {
            _logger.LogWarning("Некорректный формат команды /link от пользователя {UserId}: {Message}", 
                message.From.Id, message.Text);
            await _botClient.SendMessage(
                chatId: message.Chat.Id, 
                text: "Используйте формат: /link ссылка_на_доску");
            return;
        }

        var boardUrl = parts[1];
        var boardIdStr = boardUrl.Split('/').Last();
        
        if (!Guid.TryParse(boardIdStr, out var boardId))
        {
            _logger.LogWarning("Не удалось распарсить ID доски из сообщения: {Message}", message.Text);
            await _botClient.SendMessage(
                chatId: message.Chat.Id, 
                text: "Не удалось распознать ID доски. Убедитесь, что ссылка корректна");
            return;
        }

        _logger.LogInformation("Попытка линковки группы {GroupId} с доской {BoardId}", 
            message.Chat.Id, boardId);
            
        var linkingResult = await _groupRepository.LinkGroupToBoard(message.Chat.Id, boardId);
        if (!linkingResult.IsSuccess)
        {
            _logger.LogWarning("Не удалось слинковать группу {GroupId} с доской {BoardId}: {Error}", 
                message.Chat.Id, boardId, linkingResult.ErrorMessage);
            await _botClient.SendMessage(
                chatId: message.Chat.Id, 
                text: "Не удалось слинковать группу с доской. Убедитесь, что доска существует и у вас есть к ней доступ");
            return;
        }

        _logger.LogInformation("Успешная линковка группы {GroupId} с доской {BoardId}", 
            message.Chat.Id, boardId);
        await _botClient.SendMessage(
            chatId: message.Chat.Id, 
            text: "Группа успешно слинкована с доской! Теперь вы можете получать уведомления о задачах.");
    }

    private async Task HandleBoardInfoAsync(Message message)
    {
        _logger.LogInformation("Обработка команды /board от пользователя {UserId}", message.From?.Id);

        var boardResult = await _boardRepository.GetTaskBoardByTelegramGroupId(message.Chat.Id);
        if (!boardResult.IsSuccess)
        {
            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Не удалось найти информацию о доске. Убедитесь, что группа слинкована с доской.");
            return;
        }

        var board = boardResult.Data;
        var tasksByStatus = board.Tasks?.GroupBy(t => t.Status) ?? [];
        
        var messageText = $"📋 *Информация о доске {board.Title}*\n\n" +
                         $"👥 Участников: {board.Users?.Count ?? 0}\n" +
                         $"📝 Всего задач: {board.Tasks?.Count ?? 0}\n\n" +
                         "*Статусы задач:*\n";

        foreach (var group in tasksByStatus)
        {
            var statusName = group.Key switch
            {
                TaskStatus.ToDo => "⏳ К выполнению",
                TaskStatus.InProgress => "🚀 В работе",
                TaskStatus.OnReview => "📝 На проверке",
                TaskStatus.Approved => "✅ Выполнено",
                TaskStatus.Failed => "❌ Отклонено",
                _ => "❓ Неизвестно"
            };
            messageText += $"{statusName}: {group.Count()}\n";
        }

        await _botClient.SendMessage(
            chatId: message.Chat.Id,
            text: messageText,
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task HandleRatingAsync(Message message)
    {
        _logger.LogInformation("Обработка команды /rating от пользователя {UserId}", message.From?.Id);

        var userResult = await _userRepository.GetUserById(message.From!.Id);
        if (!userResult.IsSuccess)
        {
            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Не удалось получить информацию о вашем рейтинге.");
            return;
        }

        var user = userResult.Data;
        var messageText = $"👤 *Ваш социальный рейтинг*\n\n" +
                         $"Рейтинг: {user.SocialRating ?? 0} ⭐\n" +
                         $"Имя: {user.FirstName} {user.LastName}\n" +
                         $"Username: @{user.Username}";

        await _botClient.SendMessage(
            chatId: message.Chat.Id,
            text: messageText,
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task HandleBusyUsersAsync(Message message)
    {
        _logger.LogInformation("Обработка команды /busy от пользователя {UserId}", message.From?.Id);

        var boardResult = await _boardRepository.GetTaskBoardByTelegramGroupId(message.Chat.Id);
        if (!boardResult.IsSuccess)
        {
            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Не удалось найти информацию о доске. Убедитесь, что группа слинкована с доской.");
            return;
        }

        var busyUsers = boardResult.Data.Tasks?
            .Where(t => t.Status == TaskStatus.InProgress)
            .SelectMany(t => t.TaskWorkers ?? [])
            .DistinctBy(u => u.Id)
            .ToList() ?? [];

        if (!busyUsers.Any())
        {
            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Сейчас никто не занят выполнением задач.");
            return;
        }

        var messageText = "🚀 *Сейчас заняты:*\n\n";
        foreach (var user in busyUsers)
        {
            var tasks = boardResult.Data.Tasks?
                .Where(t => t.Status == TaskStatus.InProgress && 
                           t.TaskWorkers?.Any(w => w.Id == user.Id) == true)
                .ToList() ?? [];

            messageText += $"{user.NameToPing}\n";
            messageText = tasks.Aggregate(messageText, (current, task) => current + $"  • {task.Title}\n");
            messageText += "\n";
        }

        await _botClient.SendMessage(
            chatId: message.Chat.Id,
            text: messageText,
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task HandleHelpAsync(Message message)
    {
        _logger.LogInformation("Обработка команды /help от пользователя {UserId}", message.From?.Id);

        const string helpText = "*Доступные команды:*\n\n" +
                                "📋 */board* \\- показать информацию о доске\n" +
                                "⭐ */rating* \\- показать ваш социальный рейтинг\n" +
                                "🚀 */busy* \\- показать занятых пользователей\n" +
                                "🔗 */link* \\- привязать группу к доске\n" +
                                "❓ */help* \\- показать это сообщение";

        await _botClient.SendMessage(
            chatId: message.Chat.Id,
            text: helpText,
            parseMode: ParseMode.MarkdownV2);
    }
}