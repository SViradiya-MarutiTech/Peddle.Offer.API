using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using Application.Models;
using Domain.Dtos.Requests;
using AutoMapper;
using Domain.Dtos.Commands;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class InstantOfferController : BaseApiController
    {

        private readonly ILogger<InstantOfferController> _logger;
        private readonly IMapper _mapper;

        public InstantOfferController(ILogger<InstantOfferController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{InstantOfferId}")]
        public async Task<IActionResult> Get([FromRoute]GetInstantOfferModel model)
        {
            try
            {
                var instantOfferQueryRequest = _mapper.Map<GetInstantOfferRequest>(model);
                return Ok(await Mediator.Send(instantOfferQueryRequest));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Some thing went wrong,Exception:{ex}");
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateInstantOffer(CreateInstantOfferModel model)
        {
            try
            {
                var getInstantOfferCommandRequest = _mapper.Map<CreateInstantOfferRequest>(model);

                return CreatedAtAction(nameof(CreateInstantOffer),await Mediator.Send(getInstantOfferCommandRequest));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Some thing went wrong, Exception:{ex.Message}");
                throw;
            }
        }
    }
}
