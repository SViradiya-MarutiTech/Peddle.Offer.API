using AutoMapper;
using MediatR;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.ExternalServices;
using Domain.Dtos.ExternalServices;
using Domain.Dtos.Queries;

namespace Application.UseCases.AdditionalFees.Queries
{
   
    public class GetOfferDatabaseIdQueryEventHandler : IRequestHandler<GetOfferDatabaseIdQuery, OfferDatabaseIdDto>
    {
        private readonly IOfferOperationService _offerService;
        private readonly IMapper _mapper;
        public GetOfferDatabaseIdQueryEventHandler(IOfferOperationService offerService, IMapper mapper)
        {
            _offerService = offerService;
            _mapper = mapper;
        }

        public async Task<OfferDatabaseIdDto> Handle(GetOfferDatabaseIdQuery request, CancellationToken cancellationToken)
        {
            var getOfferDatabaseIdRequest = _mapper.Map<GetOfferDatabaseIdDto>(request);
            var offerDatabaseIds = await _offerService.GetOfferDatabaseIds(getOfferDatabaseIdRequest);
            if (offerDatabaseIds == null) throw new Exception($"OfferDatabaseIds Not Found.");
            return offerDatabaseIds;
        }

      
    }
}
