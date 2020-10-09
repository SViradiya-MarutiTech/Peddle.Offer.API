using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Dtos.Commands;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Peddle.Offer.WebApi.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthenticateController : BaseApiController
    {
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IMapper _mapper;

        public AuthenticateController(IMapper mapper, ILogger<AuthenticateController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticationRequestDto authModel)
        {
            var model = _mapper.Map<AuthanticationRequest>(authModel);
            return Ok(await Mediator.Send(model));
        }
    }
}
