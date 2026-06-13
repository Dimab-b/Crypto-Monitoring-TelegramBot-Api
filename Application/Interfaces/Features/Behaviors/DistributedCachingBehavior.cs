using ApiWithOtherApi.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using MediatR;

namespace ApiWithOtherApi.Application.Interfaces.Features.Behaviors
{
    public class DistributedCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    {
        private readonly IDistributedCache _cache;

        public DistributedCachingBehavior(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string cachedJson = await _cache.GetStringAsync(request.CacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedJson))
            {
                return JsonSerializer.Deserialize<TResponse>(cachedJson);
            }

            var response = await next();

            string jsonResponse = JsonSerializer.Serialize(response);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = request.Expiration ?? TimeSpan.FromMinutes(2)
            };

            await _cache.SetStringAsync(request.CacheKey, jsonResponse, cacheOptions, cancellationToken);

            return response;
        }
    }
}
