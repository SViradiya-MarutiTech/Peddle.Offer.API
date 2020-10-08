using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Peddle.Offer.WebApi.Controllers;
using System;
using Microsoft.Extensions.Logging;
using Application.Models;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class OfferController : BaseApiController
    {

        private ILogger<OfferController> _logger;

        public OfferController(ILogger<OfferController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetInstantOfferRequest { InstantOfferId = id }));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Some thing went wrong,Exception:{ex}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateInstantOfferRequest command)
        {
            try
            {
                return Ok(await Mediator.Send(command));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Some thing went wrong, Exception:{ex.Message}");
                throw;
            }
        }
    }
}
