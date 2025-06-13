using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Models;
using Domain.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthService(
    IUserRepository repository,
    IJwtWorkerService jwtWorkerService,
    IRefreshTokenRepository refreshTokenRepository,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<Result<TokenDto>> AuthenticateUser(UserDto user)
    {
        logger.LogInformation("Начало аутентификации пользователя {UserId}", user.Id);
        
        var userResult = await repository.UpdateOrAddUser(user);
        if (!userResult.IsSuccess)
        {
            logger.LogWarning("Не удалось обновить или добавить пользователя {UserId}: {Error}", 
                user.Id, userResult.ErrorMessage);
            return userResult.SameFailure<TokenDto>();
        }
        
        user = userResult.Data;
        logger.LogInformation("Пользователь {UserId} успешно обновлен/добавлен", user.Id);
        
        var tokensCreationResult = jwtWorkerService.CreateTokens(user);
        if (!tokensCreationResult.IsSuccess)
        {
            logger.LogError("Не удалось создать токены для пользователя {UserId}: {Error}", 
                user.Id, tokensCreationResult.ErrorMessage);
            return tokensCreationResult;
        }
        
        var addingRefreshTokenResult = await refreshTokenRepository.AddRefreshTokenAsync(tokensCreationResult.Data);
        if (!addingRefreshTokenResult.IsSuccess)
        {
            logger.LogError("Не удалось добавить refresh токен для пользователя {UserId}: {Error}", 
                user.Id, addingRefreshTokenResult.ErrorMessage);
            return tokensCreationResult.SameFailure<TokenDto>();
        }
        
        logger.LogInformation("Пользователь {UserId} успешно аутентифицирован", user.Id);
        return Result<TokenDto>.Success(tokensCreationResult.Data);
    }

    public async Task<Result<TokenDto>> RefreshAsync(TokenDto tokenDto)
    {
        logger.LogInformation("Начало обновления токена для пользователя {UserId}", tokenDto.UserId);
        
        var usingRefreshTokenResult = await refreshTokenRepository.UseRefreshTokenAsync(tokenDto.RefreshToken!);
        if (!usingRefreshTokenResult.IsSuccess)
        {
            logger.LogWarning("Не удалось использовать refresh токен: {Error}", usingRefreshTokenResult.ErrorMessage);
            return usingRefreshTokenResult.SameFailure<TokenDto>();
        }
        
        var tokensCreationResult = jwtWorkerService.CreateTokens(usingRefreshTokenResult.Data);
        if (!tokensCreationResult.IsSuccess)
        {
            logger.LogError("Не удалось создать новые токены: {Error}", tokensCreationResult.ErrorMessage);
            return tokensCreationResult;
        }
        
        var addingRefreshTokenResult = await refreshTokenRepository.AddRefreshTokenAsync(tokensCreationResult.Data);
        if (!addingRefreshTokenResult.IsSuccess)
        {
            logger.LogError("Не удалось добавить новый refresh токен: {Error}", addingRefreshTokenResult.ErrorMessage);
            return tokensCreationResult.SameFailure<TokenDto>();
        }
        
        logger.LogInformation("Токены успешно обновлены для пользователя {UserId}", tokenDto.UserId);
        return Result<TokenDto>.Success(tokensCreationResult.Data);
    }
}