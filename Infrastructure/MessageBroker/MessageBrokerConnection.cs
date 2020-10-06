using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peddle.MessageBroker.RabbitMQ.Connection;
using Peddle.Offer.Application.Interfaces.MessageBroker;
using Peddle.Offer.Domain.Dtos;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Peddle.Offer.Infrastructure.MessageBroker
{
    public class MessageBrokerConnection : IMessageBrokerConnection
    {
        public IRabbitMqConnection RabbitMQConnection { get; }
        private ILoggerFactory loggerFactory;
        private ILogger<MessageBrokerConnection> _log;
        private MessageBrokerConnectionConfiguration _configuration;

        public MessageBrokerConnection(string consumerName,
            IOptions<MessageBrokerConnectionConfiguration> configuration, ILogger<MessageBrokerConnection> log,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration.Value;
            _log = log;
            this.loggerFactory = loggerFactory;
            IConnectionFactory rabbitMqConnectionFactory = new ConnectionFactory
            {
                UserName = _configuration.RabbitMQUserName,
                Password = _configuration.RabbitMQPassword,
                HostName = _configuration.RabbitMQHostName,
                Port = _configuration.RabbitMQPortNumber,
                DispatchConsumersAsync = _configuration.DispatchConsumersAsync,
                NetworkRecoveryInterval =
                    TimeSpan.FromSeconds(_configuration.NetworkRecoveryInterval), // default is 5 seconds
                AutomaticRecoveryEnabled = _configuration.AutomaticRecoveryEnabled, // default is true
                TopologyRecoveryEnabled = _configuration.TopologyRecoveryEnabled, // default is true
                HandshakeContinuationTimeout =
                    TimeSpan.FromSeconds(_configuration.HandshakeContinuationTimeout), // 10 seconds by default
                ContinuationTimeout = TimeSpan.FromSeconds(_configuration.ContinuationTimeout), // 20 seconds by default
                RequestedConnectionTimeout =
                    _configuration.RequestedConnectionTimeout, // default value is 30 * 1000 milliseconds
                RequestedHeartbeat =
                    (ushort) _configuration.RequestedHeartbeat, // 60 seconds by default.                    
                ClientProperties = new Dictionary<string, object>
                    {{"connection-name", consumerName}}
            };

            RabbitMQConnection = new RabbitMqConnection(rabbitMqConnectionFactory,
                loggerFactory.CreateLogger<RabbitMqConnection>());
            _log.LogInformation("MessageBrokerConnection created.");
        }

        public void Dispose()
        {
            RabbitMQConnection?.Dispose();
            _log.LogInformation("MessageBrokerConnection disposed.");
        }
    }
}