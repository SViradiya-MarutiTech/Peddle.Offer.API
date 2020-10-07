using Application.Interfaces.MessageBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.EventHandlers
{
    public class MessageBrokerEventHandler : IMessageBrokerEventHandler
    {
        private readonly IMessageBrokerEventConsumer[] _consumers;
        public MessageBrokerEventHandler(IMessageBrokerEventConsumer[] consumers)
        {
            _consumers = consumers;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void StartSubscriber()
        {
            for (int i = 0; i < _consumers.Length; i++)
            {
                _consumers[i].StartSubscriber();
            }
        }

        public void SubscribeToEvent(string eventName)
        {
        }
    }
}
