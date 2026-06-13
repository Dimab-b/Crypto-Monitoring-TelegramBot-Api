using ApiWithOtherApi.Application.Interfaces.Features.Commands;
using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Infrastructure.Services;
using ApiWithOtherApi.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

namespace ApiWithOtherApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IRequestCryptoService, RequestCryptoService>();

        services.AddHttpClient("CryptoClient")
        .AddPolicyHandler(GetRetryPolicy()).AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped<ICryptoAlertService, CryptoAlertService>();
        services.AddHostedService<TelegramBotWorker>();
        return services;
        
    }
    
    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                        retryAttempt)));
    }

    static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30)
    );
    }

}

    

