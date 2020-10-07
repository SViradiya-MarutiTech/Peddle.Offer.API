using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peddle.MessageBroker.Subscriber;
using Api.MessageBrokerConsumers;
using Application.Interfaces.MessageBroker;
using Domain.Dtos;

namespace Api.MessageBroker
{
    public class MessageBrokerListener : IHostedService
    {
        private readonly ILogger<MessageBrokerListener> _log;
        private IServiceProvider serviceProvider;
        private ILoggerFactory _factory;

        public MessageBrokerListener(ILogger<MessageBrokerListener> log, IServiceProvider serviceProvider,
            ILoggerFactory factory)
        {
            _log = log;
            this.serviceProvider = serviceProvider;
            _factory = factory;
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
                var consumers = serviceProvider.GetRequiredService<IMessageBrokerEventHandler>();
                consumers.StartSubscriber();

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
            return Task.CompletedTask;
        }
    }
}