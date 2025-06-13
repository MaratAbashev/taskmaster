using AutoMapper;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;

namespace Infrastructure.Database.Repositories;

public class UserRepository(AppDbContext context, IMapper mapper) : Repository<User, long>(context), IUserRepository
{
    public async Task<Result<UserDto>> UpdateOrAddUser(UserDto userDto)
    {
        try
        {
            var user = await GetByFilterWithoutTrackingAsync(u => u.Id == userDto.Id);
            if (user == null)
            {
                await AddAsync(mapper.Map<User>(userDto));
            }
            else
            {
                mapper.Map(userDto, user);
                await context.SaveChangesAsync();
                mapper.Map(user, userDto);
            }
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception e)
        {
            return Result<UserDto>.Failure(e.Message);
        }
    }

    public async Task<Result<UserDto>> GetUserById(long userId)
    {
        try
        {
            var user = await GetByFilterWithoutTrackingAsync(u => u.Id == userId);
            return user == null
                ? Result<UserDto>.Failure("User not found", ErrorType.NotFound)
                : Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
        catch (Exception e)
        {
            return Result<UserDto>.Failure(e.Message);
        }
    }
}