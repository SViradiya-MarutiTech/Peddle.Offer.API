using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos
{
    public class AdditionalFeesDetailsDto
    {
        public decimal Amount { get; set; }
        public string OfferSalesforceId { get; set; }
        public string Source { get; set; }
    }
}
