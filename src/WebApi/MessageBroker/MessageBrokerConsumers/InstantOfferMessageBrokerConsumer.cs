using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Application.Interfaces.MessageBroker;
using AutoMapper;
using Domain.Dtos;
using Domain.Dtos.Commands;
using Domain.Dtos.MessageBroker;
using MediatR;
using Microsoft.Extensions.Logging;
using Peddle.MessageBroker.Subscriber;

namespace Api.MessageBroker.MessageBrokerConsumers
{
    public class InstantOfferMessageBrokerConsumer : IMessageBrokerEventConsumer
    {
        private readonly IMediator _mediator;
        private readonly IMessageBrokerSubscriber<RabbitMQMessage> _consumer;
        private readonly ILogger<InstantOfferMessageBrokerConsumer> _logger;
        private readonly IMapper _mapper;

        public InstantOfferMessageBrokerConsumer(
            IMediator mediator,
            IMessageBrokerSubscriber<RabbitMQMessage> consumer,
            ILogger<InstantOfferMessageBrokerConsumer> logger, IMapper mapper)
        {
            _mediator = mediator;
            _consumer = consumer;
            _logger = logger;
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
            await Task.Run(function);
        }
      
        public static T DeserializeXmlToObject<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using StringReader strReader = new StringReader(data);
            using XmlTextReader xmlReader = new XmlTextReader(strReader);
            return (T)serializer.Deserialize(xmlReader);
        }
        private async Task ProcessExchangeMessage(RabbitMQMessage message)
        {
            //based on message call _mediator.send method

            switch (message.EventType)
            {
                //TODO: use enum after integrating library
                case "instant_offer_created":
                    //use mediator for calling Service eg. IOfferService
                    await Execute(() => { _logger.LogInformation("Instant Offer Created  " + message.EventBody); });
                    break;
                case "create_instant_offer":
                    //use mediator for calling Service eg. IOfferService
                    await Execute(() =>
                    {
                        var body = DeserializeXmlToObject<InstantOfferCreatedEventBody>(message.EventBody);
                        _mediator.Send(_mapper.Map<CreateInstantOfferRequest>(body));
                        _logger.LogInformation("Create Instant Offer Called  " + message.EventBody);
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