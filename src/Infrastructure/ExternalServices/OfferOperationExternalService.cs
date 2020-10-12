using Application.Interfaces.ExternalServices;
using Domain.Dtos;
using System.Threading.Tasks;
using Domain.Dtos.ExternalServices;
using System.Linq;
using OfferOperationService;
using System.ServiceModel;

namespace Infrastructure.ExternalServices
{
    public class OfferOperationExternalService : IOfferOperationService
    {
        public OfferOperationExternalService()
        {
        }

        public IOfferService GetServiceProxy()
        {
            //https://github.com/dotnet/wcf/issues/2546
            BasicHttpBinding httpBinding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress("<YOUR SVC URL>");
            ChannelFactory<IOfferService> factory = new ChannelFactory<IOfferService>(httpBinding, endpoint);

            return factory.CreateChannel();
        }

        public async Task<OfferDatabaseIdDto> GetOfferDatabaseIds(GetOfferDatabaseIdDto getOfferDatabaseIdDto)
        {
            var request = new GetOfferDatabaseIdsRequestMessage()
            {
                //Use Peddle.Common for setting properties.
                AdminSiteUser = 1,
                RequestSource = "OfferOperationService",
                UserId = 1,
                UserTypeId = 2,
                OfferIds = new[] { getOfferDatabaseIdDto.OfferId }

            };
            var service = GetServiceProxy();
            var result = await service.GetOfferDatabaseIdsAsync(request);
            var offerDatabaseIdDto = new OfferDatabaseIdDto();
            offerDatabaseIdDto.OfferDatabaseId = result.Data.FirstOrDefault().OfferDatabaseId;

            return offerDatabaseIdDto;
        }
    }
}