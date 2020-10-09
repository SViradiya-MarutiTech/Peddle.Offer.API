using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos.MessageBroker
{
   public class InstantOfferCreatedEventBody
    {
        public int Id { get; set; }
        public double OfferAmount { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }

    }
}
