﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Configuration.Options;
using Domain.Abstractions.Services;
using Domain.Models;
using Domain.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtWorkerService(
    IOptionsMonitor<JwtOptions> options,
    ILogger<JwtWorkerService> logger) : IJwtWorkerService
{
    public Result<TokenDto> CreateTokens(UserDto userDto)
    {
        try
        {
            logger.LogInformation("Начало создания токенов для пользователя {UserId}", userDto.Id);
            
            var claims = new List<Claim>
            {
                new("UserId", userDto.Id.ToString()),
                new("Username", userDto.NameToPing)
            };

            var jwtOptions = options.CurrentValue;
            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtOptions.AccessTokenExpiryMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.IssuerSecretKey)),
                    SecurityAlgorithms.HmacSha256));

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenDto = new TokenDto()
            {
                IsUsed = false,
                UserId = userDto.Id,
                RefreshCreatedAt = DateTime.UtcNow,
                RefreshExpiresAt = DateTime.UtcNow.AddDays(options.CurrentValue.RefreshTokenExpiryDays),
                RefreshToken = Guid.NewGuid().ToString(),
                AccessToken = jwtToken
            };
            
            logger.LogInformation("Токены успешно созданы для пользователя {UserId}", userDto.Id);
            return Result<TokenDto>.Success(tokenDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании токенов для пользователя {UserId}", userDto.Id);
            return Result<TokenDto>.Failure(ex.Message);
        }
    }
}