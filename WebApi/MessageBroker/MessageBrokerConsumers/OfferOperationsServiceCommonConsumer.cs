using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Application.Interfaces.MessageBroker;
using Application.Models;
using Application.UseCases.Offers.Commands.CreateOffer;
using AutoMapper;
using Domain.Dtos;
using Domain.Dtos.MessageBroker;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peddle.MessageBroker.Subscriber;

namespace Api.MessageBroker.MessageBrokerConsumers
{
    public class OfferOperationsServiceCommonConsumer : IMessageBrokerEventConsumer
    {
        private readonly IMediator _mediator;
        private readonly IMessageBrokerSubscriber<RabbitMQMessage> _consumer;
        private readonly ILogger<OfferOperationsServiceCommonConsumer> _logger;
        private readonly MessageBrokerConnectionConfiguration _configuration;
        private readonly IMapper _mapper;

        public OfferOperationsServiceCommonConsumer(
            IMediator mediator,
            IMessageBrokerSubscriber<RabbitMQMessage> consumer,
            ILogger<OfferOperationsServiceCommonConsumer> logger,
            IOptions<MessageBrokerConnectionConfiguration> messageBrokerConfiguration, IMapper mapper)
        {
            _mediator = mediator;
            _consumer = consumer;
            _logger = logger;
            _configuration = messageBrokerConfiguration.Value;
            var routingKeys = new List<string>
            {
                "instant_offer_created",
                "create_instant_offer"
            };

            routingKeys.ForEach(SubscribeToEvent);
            _mapper = mapper;
        }

        public void StartSubscriber()
        {
            _consumer.StartSubscriber();
        }

        private async Task Execute(Action function)
        {
            await Task.Run(() =>
            {
                try
                {
                    function();
                }
                catch
                {
                }
            });
        }
      
        public static T DeserializeXmlToObject<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader strReader = new StringReader(data))
            using (XmlTextReader xmlReader = new XmlTextReader(strReader))
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }
        private async Task ProcessExchangeMessage(RabbitMQMessage message)
        {
            //based on message call _mediator.send method

            switch (message.EventType)
            {
                //TODO: use enum after integrating library
                case "instant_offer_created":
                    //use mediatr for calling Service eg. IOfferService
                    await Execute(() => { _logger.LogInformation("Offer Created  " + message.EventBody); });
                    break;
                case "create_instant_offer":
                    //use mediatr for calling Service eg. IOfferService
                    await Execute(() =>
                    {
                        var body = DeserializeXmlToObject<InstantOfferCreatedEventBody>(message.EventBody);
                        _mediator.Send(_mapper.Map<CreateInstantOfferRequest>(body));
                        _logger.LogInformation("Offer Created  " + message.EventBody);
                    });
                    break;
            }
        }

        private void SubscribeToEvent(string eventName)
        {
            _consumer.Subscribe(eventName,
                async message => { await ProcessExchangeMessage(message); });
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private void ReleaseUnmanagedResources()
        {
            _consumer?.Dispose();
        }
    }
}