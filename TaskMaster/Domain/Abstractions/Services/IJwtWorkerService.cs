using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IJwtWorkerService
{
    Result<TokenDto> CreateTokens(UserDto userDto);
}