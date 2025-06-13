using Telegram.Bot.Types;

namespace Domain.Abstractions.Services;

public interface ITelegramBotService
{
    Task HandleUpdateAsync(Update update);
}