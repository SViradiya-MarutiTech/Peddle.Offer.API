using Application.Models;
using AutoMapper;
using Domain.Dtos.MessageBroker;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InstantOfferCreatedEventBody, CreateInstantOfferRequest>();

            CreateMap<CreateInstantOfferRequest, InstantOffer>();
            CreateMap<GetInstantOfferRequest, InstantOffer>();
        }
    }
}
