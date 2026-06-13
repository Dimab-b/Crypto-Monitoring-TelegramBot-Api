using ApiWithOtherApi.Infrastructure.Services;
using Hangfire;
using MediatR;
using System.Collections;

namespace ApiWithOtherApi.Application.Interfaces.Features.Commands
{
    public class StartMonitoringCommandHandler : IRequestHandler<StartMonitoringCommand>
    {
        private readonly IRecurringJobManager _jobManeger;

        public StartMonitoringCommandHandler(IRecurringJobManager jobManeger)
        {
            _jobManeger = jobManeger;
        }

        public Task Handle(StartMonitoringCommand command , CancellationToken tk) 
        {
            try
            {
                RecurringJob.AddOrUpdate<CryptoAlertService>
                    (
                    command.jobId,
            service => service.CheckPriceAsync(command.coinName, command.targetPrice, command.jobId),
            "*/1 * * * *"

                    );

                return Unit.Task;
            }
            catch (Exception ex) { Console.WriteLine($"Виникла помилка {ex.Message}"); throw; }
        }

    }
}
