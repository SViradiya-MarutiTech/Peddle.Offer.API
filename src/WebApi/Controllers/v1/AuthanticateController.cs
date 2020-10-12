using System.Threading.Tasks;
using AutoMapper;
using Domain.Dtos.Commands;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthenticateController : BaseApiController
    {
        private readonly IMapper _mapper;

        public AuthenticateController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticationRequestDto authModel)
        {
            var model = _mapper.Map<AuthanticationRequest>(authModel);
            return Ok(await Mediator.Send(model));
        }
    }
}
