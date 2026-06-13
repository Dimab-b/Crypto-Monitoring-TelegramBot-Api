using ApiWithOtherApi.Application;
using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Infrastructure;
using ApiWithOtherApi.Infrastructure.Services;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis.StackExchange;
using Polly;
using Polly.Extensions.Http;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var baseRedisConnection = builder.Configuration["RedisConnection"] ?? "redis:6379";
var redisConnectionString = $"{baseRedisConnection},abortConnect=false,connectRetry=5";


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "RedisForCryptoApi";
});


builder.Services.AddHangfire(cfg => cfg
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseRedisStorage(redisConnectionString));

builder.Services.AddHangfireServer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddSingleton<ITelegramBotClient>(sp =>
    new TelegramBotClient(builder.Configuration["TelegramBot:Token"]));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new IDashboardAuthorizationFilter[] { }
});

app.Run();