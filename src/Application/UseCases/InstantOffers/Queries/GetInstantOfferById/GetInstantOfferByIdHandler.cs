using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Interfaces.CacheService;
using Application.Models;
using Domain.Dtos.Responses;

namespace Application.UseCases.Offers.Queries
{
    public class GetInstantOfferByIdHandler : IRequestHandler<GetInstantOfferRequest, InstantOfferModel>
    {
        private readonly ICacheService<InstantOfferModel> _cachservice;
        private readonly IInstantOfferRepository _instantOfferRepository;
        private readonly IMapper _mapper;
        public GetInstantOfferByIdHandler(ICacheService<InstantOfferModel> cachservice, IInstantOfferRepository instantOfferRepository, IMapper mapper)
        {
            _cachservice = cachservice;
            _instantOfferRepository = instantOfferRepository;
            _mapper = mapper;
        }

        public async Task<InstantOfferModel> Handle(GetInstantOfferRequest request, CancellationToken cancellationToken)
        {

            Domain.Entities.InstantOffer instantOffer = await _instantOfferRepository.GetInstantOfferById(request.InstantOfferId);

            if (instantOffer == null)
            {
                throw new Exception("Instant Offer Not Found.");
            }

            var instantOfferModel = _mapper.Map<InstantOfferModel>(instantOffer);
            return instantOfferModel;
        }
    }
}