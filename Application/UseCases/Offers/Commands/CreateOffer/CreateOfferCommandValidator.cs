using FluentValidation;
using Peddle.Offer.Domain.Entities;

namespace Peddle.Offer.Application.UseCases.Offers.Commands.CreateOffer
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
