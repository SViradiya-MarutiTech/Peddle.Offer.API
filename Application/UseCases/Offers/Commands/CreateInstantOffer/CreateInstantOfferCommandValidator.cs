using FluentValidation;
using Domain.Entities;
using Application.Models;

namespace Application.UseCases.Offers.Commands.CreateOffer
{
    public class CreateInstantOfferCommandValidator : AbstractValidator<CreateInstantOfferRequest>
    {
        public CreateInstantOfferCommandValidator()
        {
            RuleFor(p => p.OfferAmount)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .NotNull()
               .NotEqual(0);

            RuleFor(p => p.ZipCode)
              .NotEmpty().WithMessage("{PropertyName} is required.")
              .NotNull();
             
        }
    }
}
