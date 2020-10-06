using Application.UseCases.Offers.Commands.CreateOffer;
using Application.UseCases.Offers.Queries.GetOfferById;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
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
