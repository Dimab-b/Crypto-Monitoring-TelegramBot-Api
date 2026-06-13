using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Domain.Models;
using MediatR;

namespace ApiWithOtherApi.Application.Interfaces.Features.Commands
{
    public class CreateMonitoringCommandHandler : IRequestHandler<CreateMonitoringCommand , Dictionary<string, CurrencyData>>
    {
        private readonly IMediator _mediator;
        private readonly IRequestCryptoService _requestCrypto;
        private readonly ILogger<CreateMonitoringCommandHandler> _logger;

        public CreateMonitoringCommandHandler(IMediator mediator, IRequestCryptoService requestCrypto , ILogger<CreateMonitoringCommandHandler> logger)
        {
            _mediator = mediator;
            _requestCrypto = requestCrypto;
            _logger = logger;
        }

        public async Task<Dictionary<string, CurrencyData>> Handle(CreateMonitoringCommand command, CancellationToken tk) 
        {
            try
            {
                var res = await _requestCrypto.GetCurrencyPrice(command.coinName);

                await _mediator.Send(new StartMonitoringCommand (command.coinName , command.targetPrice,  command.jobId ) );

                return (res);
            }

            catch(Exception ex) { _logger.LogError($"Виникла помилка : {ex.Message}"); throw; }

        }

    }
}
