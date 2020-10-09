using Domain.Dtos.ExternalServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos.Queries
{
   public class GetOfferDatabaseIdQuery:IRequest<OfferDatabaseIdDto>
    {
        public int OfferDatabaseId { get; set; }
    }
}
