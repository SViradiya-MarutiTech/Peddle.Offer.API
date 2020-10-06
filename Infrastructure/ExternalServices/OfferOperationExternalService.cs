using OfferOperationService;
using Application.Interfaces.ExternalServices;
using Domain.Dtos;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices
{
    public class OfferOperationExternalService : IOfferOperationService
    {
        public OfferOperationExternalService()
        {
        }

        public IOfferService GetServiceProxy()
        {
            //TODO following code should be replaced after integrating  Library,It is just example, and Take URl from HashiVault.
            BasicHttpBinding httpBinding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress("http://localhost:51049/WebServices/OfferService.svc");
            ChannelFactory<IOfferService> factory = new ChannelFactory<IOfferService>(httpBinding, endpoint);

            return factory.CreateChannel();
        }

        public async Task<AdditionalFeesDetailsDto[]> GetOfferDatabaseIds()
        {
            var request = new GetAdditionalFeesDetailsRequestMessage()
            {
                AdminSiteUser = 1,
                RequestSource = "OfferOperationService",
                UserId = 1,
                UserTypeId = 2
            };
            var service = GetServiceProxy();
            var result = await service.GetAdditionalFeesDetailsAsync(request);
            var additionalFees = new AdditionalFeesDetailsDto[result.AdditionalFeesDetails.Length];
            for (int i = 0; i < result.AdditionalFeesDetails.Length; i++)
            {
                var fee = new AdditionalFeesDetailsDto();
                fee.Amount = 123;
                fee.OfferSalesforceId = "123";
                additionalFees[i] = fee;
            }

            return additionalFees;
        }
    }
}