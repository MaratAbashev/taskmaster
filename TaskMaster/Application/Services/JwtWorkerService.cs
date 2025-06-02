using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Configuration.Options;
using Domain.Models;
using Domain.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtWorkerService(IOptionsMonitor<JwtOptions> options) : IJwtWorkerService
{
    public Result<string> CreateJwtToken(UserDto userDto)
    {
        try
        {
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
                expires: DateTime.Now.AddMinutes(jwtOptions.ExpiredAtMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.IssuerSecretKey)),
                    SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Result<string>.Success(jwtToken);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }
}