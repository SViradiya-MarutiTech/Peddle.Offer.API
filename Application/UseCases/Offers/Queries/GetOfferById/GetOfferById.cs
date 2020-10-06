using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Peddle.MessageBroker.Publisher;
using Peddle.Offer.Application.Interfaces.MessageBroker;
using Peddle.Offer.Domain.Entities;
using Peddle.Offer.Domain.Dtos;
using System.Xml.Serialization;
using System.IO;
using Peddle.Offer.Application.Interfaces.Repositories;

namespace Peddle.Offer.Application.UseCases.Offers.Queries.GetOfferById
{
    public class GetOfferById : IRequest<InstantOffer>
    {
        public int Id { get; set; }
    }

    public class GetOfferByIdHandler : IRequestHandler<GetOfferById, InstantOffer>
    {
        private readonly IMapper _mapper;

        private readonly IOfferRepository _offerrepository;
        private IMessageBrokerPublisher _publisher;

        public GetOfferByIdHandler(IOfferRepository offerRepository, IMessageBrokerPublisher publisher, IMapper mapper)
        {
            _offerrepository = offerRepository;
            _publisher = publisher;
            _mapper = mapper;
        }

        //TODO: Use this function from Peddle.Common.
        private static string SerializeSoapObject(object soapObject)
        {
            if (soapObject == null)
            {
                return string.Empty;
            }

            XmlSerializer ser = new XmlSerializer(soapObject.GetType());
            string xml;

            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                ser.Serialize(ms, soapObject);
                ms.Seek(0, 0);
                xml = sr.ReadToEnd();
            }

            return xml;
        }

        public async Task<InstantOffer> Handle(GetOfferById request, CancellationToken cancellationToken)
        {
            //  var offer = await _offerrepository.GetByIdAsync(request.Id)??
            var offer = new InstantOffer();
            offer.Id = 1;
            RabbitMQMessage message = new RabbitMQMessage
            {
                EventType = "publisher_queue",
                EventBody = SerializeSoapObject(offer),
                EventTime = DateTime.UtcNow
            };
            _publisher.PublishMessageInCommonExchange(message);
            if (offer == null) throw new Exception($"Offer Not Found.");
            return offer;
        }
    }
}