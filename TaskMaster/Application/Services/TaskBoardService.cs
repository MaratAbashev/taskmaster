using AutoMapper;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Models;
using Domain.Utils;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class TaskBoardService(
    IMapper mapper, 
    ITaskBoardRepository repository,
    ILogger<TaskBoardService> logger) : ITaskBoardService
{
    public async Task<Result<TaskBoardDto>> CreateTaskBoard(long userId, string title)
    {
        logger.LogInformation("Начало создания доски задач для пользователя {UserId} с названием {Title}", 
            userId, title);
            
        var result = await repository.CreateTaskBoard(userId, title);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось создать доску задач: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Доска задач успешно создана с ID {BoardId}", result.Data.Id);
        }
        
        return result;
    }

    public async Task<Result<TaskBoardDto>> UpdateTaskBoard(Guid boardId, long userId, string newTitle)
    {
        logger.LogInformation("Начало обновления доски задач {BoardId} для пользователя {UserId}", 
            boardId, userId);
            
        var result = await repository.RenameTaskBoard(boardId, userId, newTitle);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось обновить доску задач: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Доска задач {BoardId} успешно обновлена", boardId);
        }
        
        return result;
    }

    public async Task<Result> DeleteTaskBoard(Guid boardId, long userId)
    {
        logger.LogInformation("Начало удаления доски задач {BoardId} пользователем {UserId}", 
            boardId, userId);
            
        var result = await repository.DeleteTaskBoard(boardId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось удалить доску задач: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Доска задач {BoardId} успешно удалена", boardId);
        }
        
        return result;
    }

    public async Task<Result<TaskBoardDto>> GetTaskBoard(Guid boardId, long userId)
    {
        logger.LogInformation("Запрос информации о доске задач {BoardId} пользователем {UserId}", 
            boardId, userId);
            
        var result = await repository.GetTaskBoardById(boardId, userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось получить информацию о доске задач: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Информация о доске задач {BoardId} успешно получена", boardId);
        }
        
        return result;
    }

    public async Task<Result<List<TaskBoardDto>>> GetUserTaskBoards(long userId)
    {
        logger.LogInformation("Запрос списка досок задач пользователя {UserId}", userId);
        
        var result = await repository.GetAllTaskBoardsByUserId(userId);
        
        if (!result.IsSuccess)
        {
            logger.LogWarning("Не удалось получить список досок задач: {Error}", result.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Список досок задач пользователя {UserId} успешно получен", userId);
        }
        
        return result;
    }
}