using MediatR;

namespace ApiWithOtherApi.Application.Interfaces.Features.Commands
{
    public record StartMonitoringCommand(string coinName , decimal targetPrice , string jobId) : IRequest;
    
    
}
