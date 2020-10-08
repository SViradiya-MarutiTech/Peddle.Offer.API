using System;

namespace Domain.Dtos
{

    //TODO:RabbitMQMessage should be used from RabbitMQUtility library.
    public class RabbitMQMessage
    {
        public string EventType { get; set; }
        public string EventBody { get; set; }
        public DateTime? EventTime { get; set; }
        public string RequestSource { get; set; }
    }
}