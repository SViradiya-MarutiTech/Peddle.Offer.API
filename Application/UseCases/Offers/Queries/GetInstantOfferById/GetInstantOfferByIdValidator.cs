using Application.Models;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Offers.Queries.GetInstantOfferById
{
    public class GetInstantOfferByIdValidator : AbstractValidator<GetInstantOfferRequest>
    {
        public GetInstantOfferByIdValidator()
        {
           RuleFor(p => p.InstantOfferId)
          .NotEmpty().WithMessage("{PropertyName} is required.")
          .NotNull()
          .NotEqual(0);
        }
    }
}
