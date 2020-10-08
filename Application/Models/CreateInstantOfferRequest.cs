using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Models
{
    public class CreateInstantOfferRequest : IRequest<int>
    {
        public int Id { get; set; }  
        public double OfferAmount { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }

    }

}
