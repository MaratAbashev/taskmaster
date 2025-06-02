using Api.Filters;
using Application;
using Application.Mapper;
using Application.Models;
using Application.Services;
using AutoMapper;
using Configuration.Options;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Models;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtWorkerService, JwtWorkerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile<UserMapperProfile>();
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/auth/telegram", 
        async (TelegramUserData data, IAuthService authService, IMapper mapper) =>
    {
        return await authService.AuthenticateUser(mapper.Map<UserDto>(data));
    })
    .AddEndpointFilter<ValidationFilter<TelegramUserData>>();
app.Run();