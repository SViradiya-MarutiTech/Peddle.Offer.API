using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IInstantOfferRepository : IGenericRepositoryAsync<InstantOffer>
    {
        Task AddInstantOffer(InstantOffer instantOffer);
        Task<InstantOffer> GetInstantOfferById(int instantOfferId);
    }
}
