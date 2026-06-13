using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Domain.Models;
using MediatR;
using System.Security.Cryptography.X509Certificates;

namespace ApiWithOtherApi.Application.Interfaces.Features.Commands
{
    public record GetPriceQuery(string coinName) : IRequest<Dictionary<string, CurrencyData>> , ICachedQuery
    {
        public string CacheKey => $"current-price-{coinName.ToLower()}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
    }
    
    
}
