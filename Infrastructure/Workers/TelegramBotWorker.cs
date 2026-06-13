using ApiWithOtherApi.Application.Interfaces.Features.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ApiWithOtherApi.Infrastructure.Workers
{
    public class TelegramBotWorker : BackgroundService, IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TelegramBotWorker> _logger;

        public TelegramBotWorker(
            ITelegramBotClient botClient,
            IServiceScopeFactory scopeFactory,
            ILogger<TelegramBotWorker> logger)
        {
            _botClient = botClient;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _botClient.StartReceiving(
                updateHandler: this,
                cancellationToken: stoppingToken
            );

            _logger.LogInformation("Telegram Bot Worker успішно запущено.");
        }


        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            if (update.Message is not { Text: { } text } message) return;

            if (text.StartsWith("/alarm"))
            {
                var parts = text.Split(' ');
                if (parts.Length < 3)
                {
                    await botClient.SendMessage(message.Chat.Id, "❌ Формат: /alarm <назва> <ціна>");
                    return;
                }

                var coin = parts[1];
                if (decimal.TryParse(parts[2], out var price))
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await mediator.Send(new CreateMonitoringCommand(
                            coinName: coin,
                            targetPrice: price,
                            jobId: $"tg_{message.Chat.Id}_{coin}"
                        ), ct);
                    }

                    await botClient.SendMessage(message.Chat.Id, $"✅ Аларм на {coin.ToUpper()} ({price}) встановлено!");
                }
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken ct)
        {
            _logger.LogError(exception, "Помилка в боті: {Source}", source);
            return Task.CompletedTask;
        }
    }
}