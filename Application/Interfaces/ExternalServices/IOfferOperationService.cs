using Peddle.Offer.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Peddle.Offer.Application.Interfaces.ExternalServices
{
    public interface IOfferOperationService
    {
        Task<AdditionalFeesDetailsDto[]> GetOfferDatabaseIds();
    }
}
