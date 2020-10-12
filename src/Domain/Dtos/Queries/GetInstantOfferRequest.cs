using Domain.Dtos.Responses;
using MediatR;

namespace Application.Models
{
    public class GetInstantOfferRequest : IRequest<InstantOfferModel>
    {
        public int InstantOfferId { get; set; }
    }

}
