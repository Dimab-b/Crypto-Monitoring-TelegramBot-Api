using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Domain.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace ApiWithOtherApi.Infrastructure.Services
{
    public class RequestCryptoService : IRequestCryptoService
    {
        private readonly HttpClient _httpclient;
        public async  Task<Dictionary<string, CurrencyData>> GetCurrencyPrice(string coinName) 
        {
            try
            {
                if (coinName != null)
                {
                    var info = await OnGet(coinName);

                    return info;
                }
                else { throw new ArgumentException("Назва монети не може бути порожньою", nameof(coinName)); }
            }
            catch (Exception ex) { Console.WriteLine($"Виникла помилка {ex.Message}"); throw; }

        }
        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RequestCryptoService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        

        public Dictionary<string, CurrencyData> CryptoPrice { get; set; }

        public async Task<Dictionary<string, CurrencyData>> OnGet(string coin)
        {
            var apiKey = _configuration["CryptoApiSettings:CryptoApiKey"];
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.coingecko.com/api/v3/simple/price?ids={coin}&vs_currencies=usd");
            httpRequestMessage.Headers.Add("x-cg-demo-api-key", apiKey);

            var httpClient = _httpClientFactory.CreateClient("CryptoClient");
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var jsonRaw = await httpResponseMessage.Content.ReadAsStringAsync();

                if (jsonRaw == "{}")
                {
                    throw new Exception($"API повернуло порожній результат. Перевір ID монети: {coin}");
                }

                CryptoPrice = await JsonSerializer.DeserializeAsync<Dictionary<string, CurrencyData>>(contentStream ,  new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return CryptoPrice;
                
            }

            else { throw new Exception($"API Error: {httpResponseMessage.StatusCode}");  }
        } 
    }
}

