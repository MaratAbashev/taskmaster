using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IAuthService
{
    Task<Result<string>> AuthenticateUser(UserDto user);
}