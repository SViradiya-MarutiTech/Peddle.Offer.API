using AutoMapper;
using Peddle.Offer.Application.UseCases.Offers.Commands.CreateOffer;
using Peddle.Offer.Application.UseCases.Offers.Queries.GetOfferById;
using Peddle.Offer.Domain.Entities;

namespace Peddle.Offer.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOfferCommand, InstantOffer>();
            CreateMap<GetOfferById, InstantOffer>();
        }
    }
}
