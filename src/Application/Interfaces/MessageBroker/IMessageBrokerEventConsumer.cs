using Peddle.MessageBroker.Subscriber;
using Domain.Dtos;
using System;

namespace Application.Interfaces.MessageBroker
{
    public interface IMessageBrokerEventConsumer : IDisposable
    {
        ////TODO: RabbitMQMessage should be taken from RabbitMQUtility
        //IMessageBrokerSubscriber<RabbitMQMessage> GetExchangeSubscriber(string queueName, string exchangeName);

        //IMessageBrokerSubscriber<T> GetQueueSubscriber<T>(string queueName);

        //void DisposeMessageBrokerConnection();
        void StartSubscriber();
    }
}
