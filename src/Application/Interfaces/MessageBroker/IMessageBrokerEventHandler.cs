﻿using System;

namespace Application.Interfaces.MessageBroker
{
    public interface IMessageBrokerEventHandler:IDisposable
    {
        void SubscribeToEvent(string eventName);
        void StartSubscriber();
       
    }
}
