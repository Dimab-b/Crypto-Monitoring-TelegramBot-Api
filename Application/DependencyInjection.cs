using ApiWithOtherApi.Application.Interfaces.Features.Behaviors;
using ApiWithOtherApi.Application.Interfaces.Features.Commands;
using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Infrastructure;
using ApiWithOtherApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApiWithOtherApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddOpenBehavior(typeof(DistributedCachingBehavior<,>));
            });
            
            return services;
        }
    }
}