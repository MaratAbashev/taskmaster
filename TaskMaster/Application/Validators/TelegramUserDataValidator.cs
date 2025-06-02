using System.Security.Cryptography;
using System.Text;
using Application.Models;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.Validators;

public class TelegramUserDataValidator: AbstractValidator<TelegramUserData>
{
    public TelegramUserDataValidator(IConfiguration configuration)
    {
        RuleFor(data => data)
            .NotEmpty()
            .WithMessage("User data is empty")
            .Must(data =>
            {
                var botToken = configuration["Telegram:BotToken"]!;
                var secretKey = SHA256.HashData(Encoding.UTF8.GetBytes(botToken));

                var fields = new Dictionary<string, object?>()
                {
                    ["auth_date"] = data.AuthDate,
                    ["first_name"] = data.FirstName,
                    ["id"] = data.Id,
                    ["last_name"] = data.LastName,
                    ["photo_url"] = data.PhotoUrl,
                    ["username"] = data.Username
                };
        
                var dataCheckString = fields
                    .Where(kvp => kvp.Value != null)
                    .OrderBy(kv => kv.Key)
                    .Select(kv => $"{kv.Key}={kv.Value}")
                    .ToList();

                var dataString = string.Join("\n", dataCheckString);
    
                using var hmac = new HMACSHA256(secretKey);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataString));
                var hashString = BitConverter.ToString(computedHash)
                    .Replace("-", "")
                    .ToLower();

                return hashString == data.Hash?.ToLower();
            })
            .WithMessage("Invalid Telegram auth data");
    }
}