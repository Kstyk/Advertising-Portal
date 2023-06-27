using FluentValidation;

namespace ZleceniaAPI.Models.Validators
{
    public class AddOfferDtoValidator : AbstractValidator<AddOfferDto>
    {
        public AddOfferDtoValidator()
        {
            RuleFor(e => e.Content)
                .NotEmpty()
                .WithMessage("Musisz dodać opis oferty.")
                .MaximumLength(2000)
                .WithMessage("Limit znaków dla oferty to 2000.");

            RuleFor(e => e.Price)
                .GreaterThan(0)
                .When(e => e.PriceFor != null)
                .WithMessage("Szacowany koszt usługi musi być większy od zera.");
        }
    }
}
