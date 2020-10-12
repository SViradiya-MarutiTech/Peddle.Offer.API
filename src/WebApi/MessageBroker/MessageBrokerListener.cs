using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Interfaces.MessageBroker;

namespace Api.MessageBroker
{
    public class MessageBrokerListener : IHostedService
    {
        private readonly ILogger<MessageBrokerListener> _log;
        private readonly IEnumerable<IMessageBrokerEventConsumer> _consumers;

        public MessageBrokerListener(ILogger<MessageBrokerListener> log,
            IEnumerable<IMessageBrokerEventConsumer> consumers)
        {
            _log = log;
            _consumers = consumers;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartAllConsumer();
            return Task.CompletedTask;
        }

        private void StartAllConsumer()
        {
            try
            {
                var subscribers = new Thread(StartMessageBrokerSubscribers)
                {
                    IsBackground = true
                };
                subscribers.Start();
            }
            catch (Exception exception)
            {
                _log.LogError(exception,
                    $"Failed to start message broker subscribers. \n Message: {exception.Message}");
            }
        }

        private void StartMessageBrokerSubscribers()
        {
            try
            {
                foreach (var consumer in _consumers)
                {
                    consumer.StartSubscriber();
                }
            }
            catch (Exception exception)
            {
                _log.LogError(exception,
                    $"Failed to start message broker subscribers. \n Message: {exception.Message}");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //TODO: Write logic to stop consumer
            foreach (var consumer in _consumers)
            {
                consumer.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}