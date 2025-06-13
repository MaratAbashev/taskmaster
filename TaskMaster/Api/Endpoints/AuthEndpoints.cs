using Api.Extensions;
using Api.Filters;
using Application.Models;
using AutoMapper;
using Domain.Abstractions.Services;
using Domain.Models;

namespace Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder endpoints)
    {
        var authGroup = endpoints.MapGroup("/auth")
            .WithTags("Auth");
        authGroup.MapPost("/telegram",
                async (TelegramUserData data, IAuthService authService, IMapper mapper, HttpContext context) =>
                {
                    var authResult = await authService.AuthenticateUser(mapper.Map<UserDto>(data));
                    if (!authResult.IsSuccess) return authResult.ToErrorHttpResult();
                    context.Response.Cookies.Append("refreshToken", authResult.Data.RefreshToken!, new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = authResult.Data.RefreshExpiresAt,
                        Path = "/auth/refresh",
                        SameSite = SameSiteMode.None
                    });
                    return Results.Ok(mapper.Map<AuthResult>(authResult.Data));
                })
            .AddEndpointFilter<ValidationFilter<TelegramUserData>>()
            .Produces<AuthResult>();
        authGroup.MapPost("/refresh",
            async (HttpContext context, IAuthService authService, IMapper mapper) =>
            {
                var refreshToken = context.Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                    return Results.Unauthorized();
                var refreshResult = await authService.RefreshAsync(new TokenDto { RefreshToken = refreshToken });
                if (!refreshResult.IsSuccess) 
                    return refreshResult.ToErrorHttpResult();
                context.Response.Cookies.Append("refreshToken", refreshResult.Data.RefreshToken!, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = refreshResult.Data.RefreshExpiresAt,
                    Path = "/auth/refresh",
                    SameSite = SameSiteMode.None
                });
                return Results.Ok(mapper.Map<AuthResult>(refreshResult.Data));
            });
        return endpoints;
    }
}