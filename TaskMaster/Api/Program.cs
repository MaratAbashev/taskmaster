using Api.Endpoints;
using Api.Extensions;
using Application.Mapper;
using Application.Services;
using Application.Validators;
using Configuration.Options;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IJwtWorkerService, JwtWorkerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddValidatorsFromAssemblyContaining<TelegramUserDataValidator>();
builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile<MapperProfile>();
});
builder.Services.AddCookiePolicy(opt =>
{
    opt.Secure = CookieSecurePolicy.SameAsRequest;
    opt.HttpOnly = HttpOnlyPolicy.Always;
    opt.MinimumSameSitePolicy = SameSiteMode.Strict;
});
var app = builder.Build();
app.UseCookiePolicy();
app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapAuth();
app.Run();