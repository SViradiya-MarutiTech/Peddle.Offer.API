using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Peddle.Offer.Application.Interfaces.Repositories;
using Peddle.Offer.Domain.Entities;

namespace Peddle.Offer.Application.UseCases.Offers.Commands.CreateOffer
{
    public class CreateOfferCommand : IRequest<int>
    {
        public string Title { get; set; }
    }

    public class InstantOfferCreatedEventHandler : IRequestHandler<CreateOfferCommand, int>
    {
        private readonly IGenericRepositoryAsync<InstantOffer> _offerRepository;
        private readonly IMapper _mapper;


        public InstantOfferCreatedEventHandler(IGenericRepositoryAsync<InstantOffer> offerRepository,IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            InstantOffer offer = _mapper.Map<InstantOffer>(request);
            await _offerRepository.AddAsync(offer);
            return offer.Id;

        }
    }
}
