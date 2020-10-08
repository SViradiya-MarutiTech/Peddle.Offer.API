using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public class GetInstantOfferRequest : IRequest<InstantOffer>
    {
        public int InstantOfferId{ get; set; }
    }

}
