using MediatR;
using Application.Interfaces.MessageBroker;
using System.Threading.Tasks;
using Domain.Dtos;
using Peddle.MessageBroker.Subscriber;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Api.MessageBrokerConsumers
{
    public class OfferOperationsServiceCommonConsumer : IMessageBrokerEventHandler
    {
        private readonly IMediator _mediator;
        private readonly IMessageBrokerSubscriber<RabbitMQMessage> _consumer;
        private ILogger<OfferOperationsServiceCommonConsumer> _logger;

        public OfferOperationsServiceCommonConsumer(
            IMediator mediator,
            IMessageBrokerSubscriber<RabbitMQMessage> consumer,
            ILogger<OfferOperationsServiceCommonConsumer> logger,
            MessageBrokerConnectionConfiguration messageBrokerConfiguration,
            IMessageBrokerEventConsumer messageBrokerEventConsumer
        )
        {
            _mediator = mediator;
            _consumer = consumer;
            _logger = logger;

            var routingKeys = new List<string>
            {
                "publisher_queue",
            };

            var exchange = messageBrokerConfiguration.ExchangeName;
            var queue = "OfferOperationsServiceCommonConsumer";
            _consumer = messageBrokerEventConsumer.GetExchangeSubscriber(queue, exchange);
            routingKeys.ForEach(SubscribeToEvent);
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

        private async Task ProcessExchangeMessage(RabbitMQMessage message)
        {
            //based on message call _mediator.send method

            switch (message.EventType)
            {
                //TODO: use enum after integrating library
                case "publisher_queue":
                    //use mediatr for calling Service eg. IOfferService
                    await Execute(() => { _logger.LogInformation("Consume " + message.EventBody); });
                    break;
            }
        }

        public void SubscribeToEvent(string eventName)
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