namespace ApiWithOtherApi.Application.Interfaces.Services
{
    public interface ICachedQuery
    {
        string CacheKey { get; } 
        TimeSpan? Expiration { get; } 
    }
}
