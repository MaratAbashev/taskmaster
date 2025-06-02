using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Repositories;

public interface IUserRepository
{
    Task<Result<UserDto>> UpdateOrAddUser(UserDto userDto);
    Task<Result<UserDto>> GetUserById(long userId);
}