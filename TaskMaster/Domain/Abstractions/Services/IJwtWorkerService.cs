using Domain.Models;
using Domain.Utils;

namespace Application;

public interface IJwtWorkerService
{
    Result<string> CreateJwtToken(UserDto userDto);
}