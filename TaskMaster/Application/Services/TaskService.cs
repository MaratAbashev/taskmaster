using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Models;
using Domain.Utils;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TaskStatus = Domain.Models.TaskStatus;

namespace Application.Services;

public class TaskService(
    ITaskRepository taskRepository, 
    ITaskWorkerRepository taskWorkerRepository,
    ILogger<TaskService> logger, 
    ITelegramBotClient botClient,
    ITelegramGroupRepository telegramGroupRepository) : ITaskService
{
    private async Task SendNotificationToGroup(Guid boardId, string message)
    {
        var groupResult = await telegramGroupRepository.GetGroupByBoardId(boardId);
        if (groupResult.IsSuccess)
        {
            await botClient.SendMessage(
                chatId: groupResult.Data,
                text: message);
        }
    }

    private async Task SendNotificationToTaskGroup(long taskId, string message)
    {
        var groupResult = await telegramGroupRepository.GetGroupByTaskId(taskId);
        if (groupResult.IsSuccess)
        {
            await botClient.SendMessage(
                chatId: groupResult.Data,
                text: message);
        }
    }

    public async Task<Result<TaskDto>> CreateTask(TaskDto dto, long userId)
    {
        logger.LogInformation("Начало создания задачи пользователем {UserId}", userId);
        
        var result = await taskRepository.CreateTask(dto, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось создать задачу: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Задача {TaskId} успешно создана", result.Data.Id);
            await SendNotificationToGroup(dto.BoardId, 
                $"🆕 Создана новая задача: {dto.Title}\n" +
                $"Описание: {dto.Description}\n" +
                $"Приоритет: {dto.PriorityLevel}\n" +
                $"Срок: {dto.DueDate:dd.MM.yyyy}");
        }
        
        return result;
    }
    
    public async Task<Result<TaskDto>> UpdateTask(TaskDto dto, long userId)
    {
        logger.LogInformation("Начало обновления задачи {TaskId} пользователем {UserId}", dto.Id, userId);
        
        var result = await taskRepository.UpdateTask(dto, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось обновить задачу: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Задача {TaskId} успешно обновлена", dto.Id);
            await SendNotificationToGroup(dto.BoardId, 
                $"📝 Задача обновлена: {dto.Title}\n" +
                $"Описание: {dto.Description}\n" +
                $"Приоритет: {dto.PriorityLevel}\n" +
                $"Срок: {dto.DueDate:dd.MM.yyyy}");
        }
        
        return result;
    }
    
    public async Task<Result> DeleteTask(long taskId, long userId)
    {
        logger.LogInformation("Начало удаления задачи {TaskId} пользователем {UserId}", taskId, userId);
        
        var result = await taskRepository.DeleteTask(taskId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось удалить задачу: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Задача {TaskId} успешно удалена", taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"🗑 Задача удалена");
        }
        
        return result;
    }
    
    public async Task<Result<UserDto>> FollowOnTask(long taskId, long userId)
    {
        logger.LogInformation("Пользователь {UserId} начал отслеживать задачу {TaskId}", userId, taskId);
        
        var result = await taskRepository.FollowOnTask(taskId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось начать отслеживание задачи: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Пользователь {UserId} успешно начал отслеживать задачу {TaskId}", userId, taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"👀 Пользователь {result.Data.NameToPing} начал отслеживать задачу");
        }
        
        return result;
    }
    
    public async Task<Result<UserDto>> UnfollowOnTask(long taskId, long userId)
    {
        logger.LogInformation("Пользователь {UserId} прекратил отслеживать задачу {TaskId}", userId, taskId);
        
        var result = await taskRepository.UnfollowOnTask(taskId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось прекратить отслеживание задачи: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Пользователь {UserId} успешно прекратил отслеживать задачу {TaskId}", userId, taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"👋 Пользователь {result.Data.NameToPing} прекратил отслеживать задачу");
        }
        
        return result;
    }
    
    public async Task<Result<TaskDto>> SendTaskToApprove(long taskId, long userId)
    {
        logger.LogInformation("Задача {TaskId} отправлена на утверждение пользователем {UserId}", taskId, userId);
        
        var result = await taskRepository.SendTaskToApprove(taskId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось отправить задачу на утверждение: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Задача {TaskId} успешно отправлена на утверждение", taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"📤 Задача отправлена на утверждение\n" +
                $"Исполнитель: {result.Data.Leader?.NameToPing ?? "Не назначен"}");
        }
        
        return result;
    }
    
    public async Task<Result<TaskDto>> ApproveTask(long taskId, long userId)
    {
        logger.LogInformation("Задача {TaskId} утверждена пользователем {UserId}", taskId, userId);
        
        var result = await taskRepository.ApproveTask(taskId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось утвердить задачу: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Задача {TaskId} успешно утверждена", taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"✅ Задача утверждена\n" +
                $"Утвердил: {result.Data.Leader?.NameToPing ?? "Неизвестно"}");
        }
        
        return result;
    }
    
    public async Task<Result<TaskDto>> DeclineTask(long taskId, long userId, TaskStatus status)
    {
        logger.LogInformation("Задача {TaskId} отклонена пользователем {UserId} со статусом {Status}", 
            taskId, userId, status);
        
        var result = await taskRepository.DeclineTask(taskId, userId, status);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось отклонить задачу: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Задача {TaskId} успешно отклонена", taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"❌ Задача отклонена\n" +
                $"Статус: {status}\n" +
                $"Отклонил: {result.Data.Leader?.NameToPing ?? "Неизвестно"}");
        }
        
        return result;
    }
    
    public async Task<Result<UserDto>> StartWorkingOnTask(long taskId, long userId)
    {
        logger.LogInformation("Пользователь {UserId} начал работу над задачей {TaskId}", userId, taskId);
        
        var result = await taskWorkerRepository.StartWorkingOnTask(taskId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось начать работу над задачей: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Пользователь {UserId} успешно начал работу над задачей {TaskId}", userId, taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"🚀 Пользователь {result.Data.NameToPing} начал работу над задачей");
        }
        
        return result;
    }
    
    public async Task<Result<UserDto>> FinishTask(long taskId, long userId)
    {
        logger.LogInformation("Пользователь {UserId} завершил работу над задачей {TaskId}", userId, taskId);
        
        var result = await taskWorkerRepository.FinishTask(taskId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось завершить работу над задачей: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Пользователь {UserId} успешно завершил работу над задачей {TaskId}", userId, taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"🏁 Пользователь {result.Data.NameToPing} завершил работу над задачей");
        }
        
        return result;
    }
    
    public async Task<Result<UserDto>> ConfirmUser(long taskId, long userId, long confirmerId)
    {
        logger.LogInformation("Пользователь {ConfirmerId} подтверждает пользователя {UserId} для задачи {TaskId}", 
            confirmerId, userId, taskId);
        
        var result = await taskWorkerRepository.ConfirmUser(taskId, userId, confirmerId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось подтвердить пользователя: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Пользователь {UserId} успешно подтвержден для задачи {TaskId}", userId, taskId);
            await SendNotificationToTaskGroup(taskId, 
                $"👤 Пользователь {result.Data.NameToPing} подтвержден как исполнитель задачи");
        }
        
        return result;
    }
}