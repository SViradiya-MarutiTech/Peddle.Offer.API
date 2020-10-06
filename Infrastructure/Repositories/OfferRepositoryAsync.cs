using Microsoft.EntityFrameworkCore;
using Peddle.Offer.Application.Interfaces.Repositories;
using Peddle.Offer.Domain.Entities;
using Peddle.Offer.Infrastructure.Persistence;

namespace Peddle.Offer.Infrastructure.Repositories
{
    public class OfferRepositoryAsync : GenericRepositoryAsync<InstantOffer>, IOfferRepository

    {
        private readonly DbSet<InstantOffer> _offers;

        public OfferRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _offers = dbContext.Set<InstantOffer>();
        }

        //Other  Offer Repository Methods
    }
}