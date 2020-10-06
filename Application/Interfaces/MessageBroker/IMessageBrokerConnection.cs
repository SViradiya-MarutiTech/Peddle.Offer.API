﻿using Peddle.MessageBroker.RabbitMQ.Connection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peddle.Offer.Application.Interfaces.MessageBroker
{
    public interface IMessageBrokerConnection:IDisposable
    {
        IRabbitMqConnection RabbitMQConnection { get; }

    }
}
