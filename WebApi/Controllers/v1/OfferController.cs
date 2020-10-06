using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Offers.Commands.CreateOffer;
using Application.UseCases.Offers.Queries.GetOfferById;
using Peddle.Offer.WebApi.Controllers;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class OfferController : BaseApiController
    {
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery]GetOfferById offerbyId)
        {
            return Ok(await Mediator.Send(offerbyId));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateOfferCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
