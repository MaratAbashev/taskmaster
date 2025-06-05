using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Models;
using Domain.Utils;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class AuthService(
    IUserRepository repository,
    IJwtWorkerService jwtWorkerService,
    IRefreshTokenRepository refreshTokenRepository) : IAuthService
{
    public async Task<Result<TokenDto>> AuthenticateUser(UserDto user)
    {
        var userResult = await repository.UpdateOrAddUser(user);
        if (!userResult.IsSuccess)
            return Result<TokenDto>.Failure(userResult.ErrorMessage!, userResult.ErrorType);
        user = userResult.Data;
        var tokensCreationResult = jwtWorkerService.CreateTokens(user);
        if (!tokensCreationResult.IsSuccess)
            return tokensCreationResult;
        var addingRefreshTokenResult = await refreshTokenRepository.AddRefreshTokenAsync(tokensCreationResult.Data);
        return addingRefreshTokenResult.IsSuccess 
            ? Result<TokenDto>.Success(tokensCreationResult.Data)
            : Result<TokenDto>.Failure(tokensCreationResult.ErrorMessage!, tokensCreationResult.ErrorType);
    }

    public async Task<Result<TokenDto>> RefreshAsync(TokenDto tokenDto)
    {
        var usingRefreshTokenResult = await refreshTokenRepository.UseRefreshTokenAsync(tokenDto.RefreshToken!);
        if (!usingRefreshTokenResult.IsSuccess)
            return Result<TokenDto>.Failure(usingRefreshTokenResult.ErrorMessage!, usingRefreshTokenResult.ErrorType);
        var tokensCreationResult = jwtWorkerService.CreateTokens(usingRefreshTokenResult.Data);
        if (!tokensCreationResult.IsSuccess)
            return tokensCreationResult;
        var addingRefreshTokenResult = await refreshTokenRepository.AddRefreshTokenAsync(tokensCreationResult.Data);
        return addingRefreshTokenResult.IsSuccess 
            ? Result<TokenDto>.Success(tokensCreationResult.Data)
            : Result<TokenDto>.Failure(tokensCreationResult.ErrorMessage!, tokensCreationResult.ErrorType);
    }
}