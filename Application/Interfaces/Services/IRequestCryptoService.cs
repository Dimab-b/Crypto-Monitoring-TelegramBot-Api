using ApiWithOtherApi.Domain.Models;

namespace ApiWithOtherApi.Application.Interfaces.Services
{
    public interface IRequestCryptoService
    {
        Task<Dictionary<string, CurrencyData>> GetCurrencyPrice(string coinName);
    }
}
