using FluentValidation;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Models.Validators
{
    public class EditOrderValidator : AbstractValidator<EditOrderDto>
    {
        private IUserContextService userContextService;
        public EditOrderValidator(OferiaDbContext dbContext, IUserContextService userContext)
        {
            
            RuleFor(x => x.City).NotEmpty().WithMessage("Miasto jest wymagane.");
            RuleFor(x => x.Street).NotEmpty().WithMessage("Ulica jest wymagana.");
            RuleFor(x => x.Voivodeship).NotEmpty().WithMessage("Województwo jest wymagane.");
            RuleFor(x => x.BuildingNumber).NotEmpty().WithMessage("Numer budynku jest wymagany.");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Kod pocztowy jest wymagany.");

            Voivodeship[] enumValues = (Voivodeship[])Enum.GetValues(typeof(Voivodeship));
            List<String> enums = new List<string>();
            foreach (Voivodeship v in enumValues)
            {
                enums.Add(v.ToString());
            }

            RuleFor(x => x.Voivodeship)
                .NotNull()
                .Must(value => enums.Contains(value))
                    .WithMessage("Nieprawidłowe województwo.");

            RuleFor(dto => dto.Description)
                .NotEmpty()
                .WithMessage("Opis jest wymagany.");

            RuleFor(dto => dto.Title)
                .NotEmpty()
                .WithMessage("Tytuł jest wymagany.");

            RuleFor(dto => dto.Budget)
                .GreaterThanOrEqualTo(0);

            RuleFor(dto => dto.CategoryId)
                .GreaterThanOrEqualTo(0)
                .NotEmpty()
                .WithMessage("Musisz wybrać kategorię zlecenia.");
        }
    }
}
