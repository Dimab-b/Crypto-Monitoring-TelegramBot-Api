using ApiWithOtherApi.Domain.Models;
using MediatR;

namespace ApiWithOtherApi.Application.Interfaces.Features.Commands
{
    public record CreateMonitoringCommand (string coinName, decimal targetPrice, string jobId) : IRequest<Dictionary<string, CurrencyData>>;    
}
