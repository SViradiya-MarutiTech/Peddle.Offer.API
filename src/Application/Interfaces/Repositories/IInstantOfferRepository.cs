using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IInstantOfferRepository : IGenericRepositoryAsync<InstantOffer>
    {
        void AddInstantOffer(InstantOffer instantOffer);
        InstantOffer GetInstantOfferById(int instantOfferId);
    }
}
