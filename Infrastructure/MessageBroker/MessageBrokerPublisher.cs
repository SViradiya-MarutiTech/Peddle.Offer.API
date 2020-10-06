using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peddle.MessageBroker.RabbitMQ.Publisher;
using Peddle.MessageBroker.Serializer;
using Peddle.Offer.Application.Interfaces.MessageBroker;
using Peddle.Offer.Domain.Dtos;

namespace Peddle.Offer.Infrastructure.MessageBroker
{
    public class MessageBrokerPublisher : IMessageBrokerPublisher
    {
        private readonly ILogger _log;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMessageBrokerConnection _messageBrokerPublisherConnection;
        private readonly MessageBrokerConnectionConfiguration _configurationSettings;

        public MessageBrokerPublisher(IMessageBrokerConnection messageBrokerConnection, ILoggerFactory loggerFactory,
            IOptions<MessageBrokerConnectionConfiguration> configuration)
        {
            _messageBrokerPublisherConnection = messageBrokerConnection;
            _loggerFactory = loggerFactory;
            _log = _loggerFactory.CreateLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
            _configurationSettings = configuration.Value;
        }

        public void PublishMessageInTopicExchange(RabbitMQMessage rabbitMqMessage)
        {
        }
        public void PublishMessageInCommonExchange(RabbitMQMessage rabbitMqMessage)
        {
            var exchange = _configurationSettings.ExchangeName;
            PublishMessageInExchange(rabbitMqMessage, exchange);
        }
        
        public void PublishMessagesInRetryExchange(RabbitMQMessage message, int time, string retryQueueName,
            string routingKey
            , string deadLetterExchangeName, string exchangeName)
        {
            try
            {
                //TODO: Remove after Peddle.Common library is integrated
                message.RequestSource = "OfferOperationsService";

                var exchangePublisher = new DeadLetterQueuePublisher<RabbitMQMessage>(
                    _messageBrokerPublisherConnection.RabbitMQConnection,
                    deadLetterExchangeName, exchangeName, retryQueueName, routingKey.ToLower(), time,
                    _loggerFactory.CreateLogger<DeadLetterQueuePublisher<RabbitMQMessage>>(),
                    new XmlSerializer<RabbitMQMessage>()
                );
                exchangePublisher.Publish(message);
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, null, $"Failed to Publish message in {exchangeName} exchange");
            }
        }

        public void PublishMessageInQueue<T>(T rabbitMqMessage, string queueName)
        {
            try
            {
                var exchangePublisher = new QueuePublisher<T>(
                    _messageBrokerPublisherConnection.RabbitMQConnection,
                    queueName, _loggerFactory.CreateLogger<QueuePublisher<T>>(),
                    new XmlSerializer<T>());
                exchangePublisher.Publish(rabbitMqMessage);
                _log.LogInformation($"Successfully published {queueName} RabbitMq event. {rabbitMqMessage}");
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, null, $"Failed to Publish message in {queueName} queue");
            }
        }

        public void PublishMessageInExchange(RabbitMQMessage message, string exchange)
        {
            try
            {
                //TODO: Remove after Peddle.Common library is integrated
                message.RequestSource = "OfferOperationsService";
                var exchangePublisher = new ExchangePublisher<RabbitMQMessage>(
                    _messageBrokerPublisherConnection.RabbitMQConnection,
                    exchange, message.EventType.ToLower(),
                    _loggerFactory.CreateLogger<ExchangePublisher<RabbitMQMessage>>(),
                    new XmlSerializer<RabbitMQMessage>()
                );
                exchangePublisher.Publish(message);
            }
            catch (Exception exception)
            {
            }
        }
    }
}