using Api.Models;
using Application.Models;
using AutoMapper;
using Domain.Dtos;
using Domain.Dtos.Commands;
using Domain.Dtos.MessageBroker;
using Domain.Dtos.Queries;
using Domain.Dtos.Requests;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Create Instant Offer Mapping
            CreateMap<CreateInstantOfferModel, CreateInstantOfferRequest>().ReverseMap();
            CreateMap<InstantOfferCreatedEventBody, CreateInstantOfferRequest>().ReverseMap();
            CreateMap<CreateInstantOfferRequest, InstantOffer>().ReverseMap();
            CreateMap<InstantOffer, Domain.Dtos.Responses.InstantOfferModel>().ReverseMap();

            CreateMap<GetInstantOfferModel, GetInstantOfferRequest>().ReverseMap();
            CreateMap<GetInstantOfferRequest, InstantOffer>().ReverseMap();

            //Offer DatabaseId Mapping
            CreateMap<GetOfferDatabaseIdDto, GetOfferDatabaseIdQuery>().ReverseMap();
        }
    }
}
