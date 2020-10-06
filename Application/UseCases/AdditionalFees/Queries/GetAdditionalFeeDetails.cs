using AutoMapper;
using MediatR;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.ExternalServices;

namespace Application.UseCases.AdditionalFees.Queries
{
    public class GetAdditionalFeeDetails : IRequest<AdditionalFeesDetailsDto[]>
    {
    }
    public class GetAdditionalFeeDetailsQueryHandler : IRequestHandler<GetAdditionalFeeDetails, AdditionalFeesDetailsDto[]>
    {
        private readonly IOfferOperationService _offerService;
        public GetAdditionalFeeDetailsQueryHandler(IOfferOperationService offerService)
        {
            _offerService = offerService;
        }

        public async Task<AdditionalFeesDetailsDto[]> Handle(GetAdditionalFeeDetails request, CancellationToken cancellationToken)
        {
            var additionalfees = await _offerService.GetOfferDatabaseIds();
            if (additionalfees == null) throw new Exception($"AdditionalFees Not Found.");
            return additionalfees;
        }


    }
}
