﻿using Peddle.MessageBroker.Subscriber;
using Peddle.Offer.Domain.Dtos;

namespace Peddle.Offer.Application.Interfaces.MessageBroker
{
    public interface IMessageBrokerEventConsumer
    {
        //TODO: RabbitMQMessage should be taken from RabbitMQUtility
        IMessageBrokerSubscriber<RabbitMQMessage> GetExchangeSubscriber(string queueName, string exchangeName);

        IMessageBrokerSubscriber<T> GetQueueSubscriber<T>(string queueName);

        void DisposeMessageBrokerConnection();
    }
}
