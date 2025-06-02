using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Models;
using Domain.Utils;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class AuthService(
    IUserRepository repository,
    IJwtWorkerService jwtWorkerService) : IAuthService
{
    public async Task<Result<string>> AuthenticateUser(UserDto user)
    {
        var userResult = await repository.UpdateOrAddUser(user);
        if (!userResult.IsSuccess)
            return Result<string>.Failure(userResult.ErrorMessage!, userResult.ErrorType);
        user = userResult.Data;
        return jwtWorkerService.CreateJwtToken(user);
    }
}