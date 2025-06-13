using Api.Endpoints;
using Api.Extensions;
using Application.Mapper;
using Application.Services;
using Application.Validators;
using Domain.Abstractions.Services;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using Serilog.Events;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<TelegramUserDataValidator>();
builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile<InfrastructureMapperProfile>();
    opt.AddProfile<TaskBoardMapperProfile>();
    opt.AddProfile<TokenMapperProfile>();
});
builder.Services.AddCookiePolicy(opt =>
{
    opt.Secure = CookieSecurePolicy.None;
    opt.HttpOnly = HttpOnlyPolicy.Always;
    opt.MinimumSameSitePolicy = SameSiteMode.None;
}); 
builder.Services.AddScoped<ITaskBoardService, TaskBoardService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(_ =>
{
    var botToken = builder.Configuration["Telegram:BotToken"];
    if (string.IsNullOrEmpty(botToken))
        throw new NullReferenceException("Telegram Bot Token is empty");
    var botClient = new TelegramBotClient(botToken);
    botClient.SetWebhook("https://task-master.cloudpub.ru/bot",
        allowedUpdates: [UpdateType.Message],
        dropPendingUpdates: true);
    return botClient;
});
builder.Services.AddScoped<ITelegramBotService, TelegramBotService>();
var app = builder.Build();
app.UseSerilogRequestLogging();

app.UseCookiePolicy();
app.UseCors("AllowFrontend");
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Map("/api", apiApp =>
{
    apiApp.UseEndpoints(endpoints =>
    {
        endpoints.MapAuth();
        endpoints.MapTaskBoardEndpoints();
        endpoints.MapTaskEndpoints();
    });
});
app.MapPost("/bot", async (Update update, ITelegramBotService botService) =>
{
    await botService.HandleUpdateAsync(update);
    return Results.Ok();
});
app.MapWhen(context => !context.Request.Path.StartsWithSegments("/api"), spaApp =>
{
    spaApp.UseEndpoints(endpoints =>
    {
        endpoints.MapFallbackToFile("index.html");
    });
});
try
{
    Log.Information("Запуск приложения...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение завершилось с ошибкой");
}
finally
{
    Log.CloseAndFlush();
}