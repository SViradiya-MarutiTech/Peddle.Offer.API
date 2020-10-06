using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Domain.Entities;

namespace Infrastructure.Repositories
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