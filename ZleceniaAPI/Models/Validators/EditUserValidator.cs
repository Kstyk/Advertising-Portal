using FluentValidation;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Models.Validators
{
    public class EditUserValidator : AbstractValidator<EditUserDto>
    {
        private IUserContextService userContextService;
        public EditUserValidator(OferiaDbContext dbContext, IUserContextService userContext)
        {
            userContextService = userContext;
            var userId = userContextService.GetUserId;

            RuleFor(x => x.PhoneNumber)
                .Custom((value, context) =>
                {
                    var phoneInUse = dbContext.Users.Any(u => u.PhoneNumber == value && userId != u.Id);

                    if (phoneInUse)
                    {
                        context.AddFailure("PhoneNumber", "Ten numer telefonu istnieje w naszej bazie.");
                    }
                });

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Imię jest wymagane.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Nazwisko jest wymagane.");
            RuleFor(x => x.City).NotEmpty().WithMessage("Miasto jest wymagane.");
            RuleFor(x => x.Street).NotEmpty().WithMessage("Ulica jest wymagana.");
            RuleFor(x => x.Voivodeship).NotEmpty().WithMessage("Województwo jest wymagane.");
            RuleFor(x => x.BuildingNumber).NotEmpty().WithMessage("Numer budynku jest wymagany.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Numer telefonu jest wymagany.");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Kod pocztowy jest wymagany.");
            RuleFor(x => x.StatusOfUserId).NotEmpty().WithMessage("Musisz wybrać status użytkownika.")
                .Custom((value, context) =>
                {
                    var statusExists = dbContext.StatusOfUsers.Any(s => s.Id == value);

                    if (!statusExists)
                    {
                        context.AddFailure("StatusOfUserId", "Nieprawidłowy identyfikator statusu użytkownika.");
                    }
                });
        

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotNull() // Dodane dla wartości nullable (string?)
                .NotEmpty()
                .WithMessage("Pole opisu firmy nie może być puste.");

            RuleFor(x => x.CompanyName)
                .Cascade(CascadeMode.Stop)
                .NotNull() // Dodane dla wartości nullable (string?)
                .NotEmpty()
                .When(x => x.StatusOfUserId == 1)
                .WithMessage("Nazwa firmy jest wymagana.");

            RuleFor(x => x.TaxIdentificationNumber)
                .Cascade(CascadeMode.Stop)
                .NotNull() // Dodane dla wartości nullable (string?)
                .NotEmpty()
                .When(x => x.StatusOfUserId == 1)
                .WithMessage("Numer NIP jest wymagany.");


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
        }
    }
}
