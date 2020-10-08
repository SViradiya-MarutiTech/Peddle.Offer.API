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
        private List<InstantOffer> _instantOffersList { get; set; }
        public OfferRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _instantOffers = dbContext.Set<InstantOffer>();
            _instantOffersList = MockData();
        }

        private List<InstantOffer> MockData()
        {
            return new List<InstantOffer> {

            new InstantOffer{
                Id=1,
                City="Daviston",
                OfferAmount=400,
                State="AL",
                ZipCode=36256
            },
            new InstantOffer{
                Id=2,
                City="Princeton",
                OfferAmount=250,
                State="24740",
                ZipCode=24740
            },
            new InstantOffer{
                Id=3,
                City="Tarboro",
                OfferAmount=150,
                State="NC",
                ZipCode=27886
            },
            new InstantOffer{
                Id=4,
                City="Victor",
                OfferAmount=600,
                State="NY",
                ZipCode=14564
            },
            new InstantOffer{
                Id=5,
                City="Corbin",
                OfferAmount=250,
                State="KY",
                ZipCode=40701
            },


            };
        }

        public List<InstantOffer> GetOffers()
        {
            return _instantOffersList;
        }
        public InstantOffer GetInstantOfferById(int instantOfferId)
        {
            return _instantOffersList.Find(i=>i.Id==instantOfferId);
        }
        public  void AddInstantOffer(InstantOffer instantOffer)
        {
             if(!_instantOffersList.Any(i=>i.Id==instantOffer.Id))
            {
                _instantOffersList.Add(instantOffer);
            }

        }
        //Other  Offer Repository Methods
    }
}