using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Domain.Models;
using MediatR;

namespace ApiWithOtherApi.Application.Interfaces.Features.Commands
{
    public class GetPriceQueryHandler : IRequestHandler<GetPriceQuery , Dictionary<string, CurrencyData>>
    {
        private readonly IRequestCryptoService _requestCryptoService;
        public GetPriceQueryHandler (IRequestCryptoService requestCryptoService) { _requestCryptoService = requestCryptoService; }

        public async Task<Dictionary<string, CurrencyData>> Handle(GetPriceQuery command , CancellationToken token)
        {
            try
            {
                var currentPrice = await _requestCryptoService.GetCurrencyPrice(command.coinName);

                return currentPrice;
            }
            catch (Exception ex) { Console.WriteLine($"Виникла помилка {ex.Message}"); throw; }
        }
        
    }
}
