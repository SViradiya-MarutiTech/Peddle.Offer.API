using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Interfaces;
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
            var offerCache = _cachservice.GetItem(request.InstantOfferId.ToString());
            if (offerCache != null)
            {
                return _mapper.Map<InstantOfferModel>(offerCache);
            }
            //Call Repository here.
            // var offer = await _offerrepository.GetByIdAsync(request.Id);

            var instantOffer = _instantOfferRepository.GetInstantOfferById(request.InstantOfferId);

            if (instantOffer == null) throw new Exception($"Instant Offer Not Found.");
            var instantOfferModel = _mapper.Map<InstantOfferModel>(instantOffer);

            _cachservice.UpsertItem(request.InstantOfferId.ToString(), instantOfferModel);
            return instantOfferModel;
        }
    }
}