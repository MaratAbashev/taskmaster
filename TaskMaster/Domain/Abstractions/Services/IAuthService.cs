using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IAuthService
{
    Task<Result<TokenDto>> AuthenticateUser(UserDto user);
    Task<Result<TokenDto>> RefreshAsync(TokenDto tokenDto);
}