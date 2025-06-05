using AutoMapper;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database.Repositories;

public class RefreshTokenRepository(AppDbContext context,
    IMapper mapper) : Repository<RefreshToken, long>(context), IRefreshTokenRepository
{
    public async Task<Result<UserDto>> UseRefreshTokenAsync(string refreshTokenValue)
    {
        try
        {
            var refreshToken = await _dbSet
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshTokenValue);
            if (refreshToken == null)
                return Result<UserDto>.Failure("RefreshToken not found", ErrorType.Unauthorized);
            if (refreshToken.IsUsed)
                return Result<UserDto>.Failure("RefreshToken is used", ErrorType.Unauthorized);
            if (refreshToken.ExpiresAt < DateTime.UtcNow)
                return Result<UserDto>.Failure("RefreshToken has expired", ErrorType.Unauthorized);
            refreshToken.IsUsed = true;
            await context.SaveChangesAsync();
            return Result<UserDto>.Success(mapper.Map<UserDto>(refreshToken.User));
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
    }

    public async Task<Result> AddRefreshTokenAsync(TokenDto tokenDto)
    {
        try
        {
            await AddAsync(mapper.Map<RefreshToken>(tokenDto));
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}