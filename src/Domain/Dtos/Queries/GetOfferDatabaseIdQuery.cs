using Domain.Dtos.ExternalServices;
using MediatR;

namespace Domain.Dtos.Queries
{
   public class GetOfferDatabaseIdQuery:IRequest<OfferDatabaseIdDto>
    {
        public int OfferDatabaseId { get; set; }
    }
}
