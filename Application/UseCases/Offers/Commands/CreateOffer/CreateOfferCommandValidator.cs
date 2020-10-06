using FluentValidation;
using Domain.Entities;

namespace Application.UseCases.Offers.Commands.CreateOffer
{
    public class CreateOfferCommandValidator : AbstractValidator<InstantOffer>
    {
        public CreateOfferCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotNull();

        }
    }
}
