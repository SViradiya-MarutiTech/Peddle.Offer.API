using System;

namespace Application.Interfaces.MessageBroker
{
    public interface IMessageBrokerEventConsumer : IDisposable
    {
        void StartSubscriber();
    }
}
