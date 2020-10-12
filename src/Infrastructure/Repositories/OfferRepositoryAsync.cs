using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OfferRepositoryAsync : GenericRepositoryAsync<InstantOffer>, IInstantOfferRepository

    {
        private readonly DbSet<InstantOffer> _instantOffers;
        private List<InstantOffer> InstantOffersList { get; set; }

        public OfferRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _instantOffers = dbContext.Set<InstantOffer>();
            InstantOffersList = MockData();
        }

        private List<InstantOffer> MockData()
        {
            return new List<InstantOffer>
            {
                new InstantOffer
                {
                    Id = 1,
                    City = "Daviston",
                    OfferAmount = 400,
                    State = "AL",
                    ZipCode = 36256
                },
                new InstantOffer
                {
                    Id = 2,
                    City = "Princeton",
                    OfferAmount = 250,
                    State = "24740",
                    ZipCode = 24740
                },
                new InstantOffer
                {
                    Id = 3,
                    City = "Tarboro",
                    OfferAmount = 150,
                    State = "NC",
                    ZipCode = 27886
                },
                new InstantOffer
                {
                    Id = 4,
                    City = "Victor",
                    OfferAmount = 600,
                    State = "NY",
                    ZipCode = 14564
                },
                new InstantOffer
                {
                    Id = 5,
                    City = "Corbin",
                    OfferAmount = 250,
                    State = "KY",
                    ZipCode = 40701
                },
            };
        }

        #region Mocking Methods

        public List<InstantOffer> GetInstantOffers()
        {
            return InstantOffersList;
        }

        public async Task<InstantOffer> GetInstantOfferById(int instantOfferId)
        {
            return InstantOffersList.Find(i => i.Id == instantOfferId);
        }

        public async Task AddInstantOffer(InstantOffer instantOffer)
        {
            if (InstantOffersList.All(i => i.Id != instantOffer.Id))
            {
                InstantOffersList.Add(instantOffer);
            }
        }

        #endregion
    }
}