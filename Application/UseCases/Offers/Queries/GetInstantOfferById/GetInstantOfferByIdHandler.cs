using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Peddle.MessageBroker.Publisher;
using Domain.Entities;
using Domain.Dtos;
using System.Xml.Serialization;
using System.IO;
using Application.Interfaces.MessageBroker;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.Models;

namespace Application.UseCases.Offers.Queries
{
    public class GetInstantOfferByIdHandler : IRequestHandler<GetInstantOfferRequest, InstantOffer>
    {
        private readonly ICacheService<InstantOffer> _cachservice;
        private readonly IInstantOfferRepository _instantOfferRepository;
        public GetInstantOfferByIdHandler(ICacheService<InstantOffer> cachservice, IInstantOfferRepository instantOfferRepository)
        {
            _cachservice = cachservice;
            _instantOfferRepository = instantOfferRepository;
        }

        public async Task<InstantOffer> Handle(GetInstantOfferRequest request, CancellationToken cancellationToken)
        {
            var offerCache = _cachservice.GetItem(request.InstantOfferId.ToString());
            if (offerCache != null)
            {
                return offerCache;
            }
            //Call Repository here.
            // var offer = await _offerrepository.GetByIdAsync(request.Id);

            var instantOffer = _instantOfferRepository.GetInstantOfferById(request.InstantOfferId);

            if (instantOffer == null) throw new Exception($"Instant Offer Not Found.");

            _cachservice.UpsertItem(request.InstantOfferId.ToString(), instantOffer);
            return instantOffer;
        }
    }
}