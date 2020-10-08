using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Domain.Dtos;
using Application.Interfaces.MessageBroker;
using System;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Application.Models;

namespace Application.UseCases.Offers.Commands
{
   
    public class CreateInstantOfferEventHandler : IRequestHandler<CreateInstantOfferRequest, int>
    {
        private readonly IInstantOfferRepository _offerRepository;
        private readonly IMapper _mapper;
        private IMessageBrokerPublisher _publisher;


        public CreateInstantOfferEventHandler(IInstantOfferRepository offerRepository, IMapper mapper, IMessageBrokerPublisher publisher)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
            _publisher = publisher;
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
        public static string SerializeObjectToXml(object soapObject)
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
        public async Task<int> Handle(CreateInstantOfferRequest request, CancellationToken cancellationToken)
        {
            InstantOffer offer = _mapper.Map<InstantOffer>(request);

            _offerRepository.AddInstantOffer(offer);

            //Publish event that offer is created 
            RabbitMQMessage message = new RabbitMQMessage
            {
                EventType = "instant_offer_created",
                EventBody = SerializeObjectToXml(offer),
                EventTime = DateTime.UtcNow
            };
            _publisher.PublishMessageInCommonExchange(message);
            return offer.Id;

        }
    }
}
