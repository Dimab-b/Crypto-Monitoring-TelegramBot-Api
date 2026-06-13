using ApiWithOtherApi.Application.Interfaces.Services;
using Hangfire;
using System.Drawing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Configuration;

namespace ApiWithOtherApi.Infrastructure.Services
{
    public class CryptoAlertService : ICryptoAlertService
    {
        private readonly IRequestCryptoService _cryptoService;
        private readonly ILogger<CryptoAlertService> _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly IConfiguration _configuration;
        public CryptoAlertService(IRequestCryptoService cryptoService, ILogger<CryptoAlertService> logger , IConfiguration configuration) { _cryptoService = cryptoService;  _logger = logger; _configuration = configuration; var token = configuration["TelegramBot:Token"] ;
            _botClient = new TelegramBotClient(token);
        }

        public async Task CheckPriceAsync(string coinName, decimal targetPrice , string jobId)
        {
            try
            {
                var data = await _cryptoService.GetCurrencyPrice(coinName);

                var currentPrice = data[coinName.ToLower()].Usd;

                _logger.LogInformation($"Перевірка {coinName}: ціна {currentPrice}, ціль {targetPrice}");

                if (currentPrice >= targetPrice)
                {
                    _logger.LogInformation($"УВАГА! Ціна {coinName} досягла {currentPrice}!");
                    var chatId = _configuration["TelegramBot:ChatId"];
                    await _botClient.SendMessage(chatId, $"🚀 УВАГА! Ціна {coinName} досягла {currentPrice} USD!");
                    RecurringJob.RemoveIfExists(jobId);
                }
            }

            catch (Exception ex) { _logger.LogError($"Виникла помилка {ex.Message}"); throw; }
        }
    }
}
