using Domain.Dtos.Responses;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public class GetInstantOfferRequest : IRequest<InstantOfferModel>
    {
        public int InstantOfferId { get; set; }
    }

}
