using ApiWithOtherApi.Application.Interfaces.Features.Commands;
using ApiWithOtherApi.Application.Interfaces.Services;
using ApiWithOtherApi.Domain.Models;
using ApiWithOtherApi.Infrastructure.Services;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithOtherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CryptoController (IMediator mediator) { _mediator = mediator; }

        [HttpGet("GetPrice")]
        public async Task<ActionResult<Dictionary<string , CurrencyData>>> GetInfo([FromQuery]GetPriceQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        [HttpGet("GetPriceWithAlarm")]
        public async Task<ActionResult<Dictionary<string ,CurrencyData>>> GetInfoWithAlarmUp([FromQuery]CreateMonitoringCommand command )
        {
            var res = await _mediator.Send(command);

            
            return Accepted(res);
        }
    }
}
