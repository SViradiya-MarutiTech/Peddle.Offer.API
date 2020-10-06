using System;

namespace Peddle.Offer.Domain.Dtos
{
    public class RabbitMQMessage
    {
        public string EventType { get; set; }
        public string EventBody { get; set; }
        public DateTime? EventTime { get; set; }
        public string RequestSource { get; set; }
    }
}