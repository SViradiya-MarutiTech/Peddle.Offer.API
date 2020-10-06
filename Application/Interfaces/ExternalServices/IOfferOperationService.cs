using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ExternalServices
{
    public interface IOfferOperationService
    {
        Task<AdditionalFeesDetailsDto[]> GetOfferDatabaseIds();
    }
}
