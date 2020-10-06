using Peddle.Offer.Domain.Dtos;

namespace Peddle.Offer.Application.Interfaces.MessageBroker
{
    public interface IMessageBrokerPublisher
    {
        void PublishMessageInTopicExchange(RabbitMQMessage rabbitMqMessage);
        void PublishMessageInCommonExchange(RabbitMQMessage rabbitMqMessage);

        void PublishMessagesInRetryExchange(RabbitMQMessage message, int time, string retryQueueName,
            string routingKey
            , string deadLetterExchangeName, string exchangeName);

        void PublishMessageInQueue<T>(T rabbitMqMessage, string queueName);
        void PublishMessageInExchange(RabbitMQMessage message, string exchange);
    }
}