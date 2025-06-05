using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<Result<UserDto>> UseRefreshTokenAsync(string refreshTokenValue);
    Task<Result> AddRefreshTokenAsync(TokenDto tokenDto);
}