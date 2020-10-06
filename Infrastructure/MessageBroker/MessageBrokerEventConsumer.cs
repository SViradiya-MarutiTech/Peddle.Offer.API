﻿using Microsoft.Extensions.Logging;
using Peddle.MessageBroker.RabbitMQ.Subscriber;
using Peddle.MessageBroker.Serializer;
using Peddle.MessageBroker.Subscriber;
using Peddle.Offer.Application.Interfaces.MessageBroker;
using Peddle.Offer.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peddle.Offer.Infrastructure.MessageBroker
{
    public class MessageBrokerEventConsumer : IMessageBrokerEventConsumer
    {
        private readonly IMessageBrokerConnection _messageBrokerConnection;
        private readonly ILoggerFactory _loggerFactory;

        public MessageBrokerEventConsumer(IMessageBrokerConnection messageBrokerConnection,
            ILoggerFactory loggerFactory)
        {
            _messageBrokerConnection = messageBrokerConnection;
            _loggerFactory = loggerFactory;
        }
        public IMessageBrokerSubscriber<RabbitMQMessage> GetExchangeSubscriber(string queueName, string exchangeName)
        {
            var messageBrokerSubscriber =
                (IMessageBrokerSubscriber<RabbitMQMessage>) new ExchangeSubscriber<RabbitMQMessage>(
                    _messageBrokerConnection.RabbitMQConnection,
                    queueName,
                    exchangeName,
                    _loggerFactory.CreateLogger<ExchangeSubscriber<RabbitMQMessage>>(),
                    new XmlSerializer<RabbitMQMessage>());
            return messageBrokerSubscriber;
        }

        public IMessageBrokerSubscriber<T> GetQueueSubscriber<T>(string queueName)
        {
            var messageBrokerSubscriber = (IMessageBrokerSubscriber<T>) new QueueSubscriber<T>(
                _messageBrokerConnection.RabbitMQConnection,
                queueName,
                _loggerFactory.CreateLogger<QueueSubscriber<T>>(),
                new XmlSerializer<T>());
            return messageBrokerSubscriber;
        }

        public void DisposeMessageBrokerConnection()
        {
            _messageBrokerConnection.Dispose();
        }
    }
}