using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos
{
    public class MessageBrokerConnectionConfiguration
    {
        public string RabbitMQUserName { get; set; }
        public string RabbitMQPassword { get; set; }
        public string RabbitMQHostName { get; set; }
        public int RabbitMQPortNumber { get; set; }
        public bool DispatchConsumersAsync { get; set; } = true;
        public int NetworkRecoveryInterval { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public bool TopologyRecoveryEnabled { get; set; } = true;
        public int HandshakeContinuationTimeout { get; set; }
        public int ContinuationTimeout { get; set; }
        public int RequestedConnectionTimeout { get; set; }
        public int RequestedHeartbeat { get; set; } 
        public Dictionary<string, string> ClientProperties { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
    }
}