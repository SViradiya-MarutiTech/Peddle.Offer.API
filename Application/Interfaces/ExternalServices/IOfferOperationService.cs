using Domain.Dtos;
using Domain.Dtos.ExternalServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ExternalServices
{
    public interface IOfferOperationService
    {
        Task<OfferDatabaseIdDto> GetOfferDatabaseIds(GetOfferDatabaseIdDto offerDatabaseIdDto);
    }
}
