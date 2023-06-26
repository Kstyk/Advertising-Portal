using FluentValidation;
using System;
using System.Linq;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(OferiaDbContext dbContext)
        {
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Niepoprawny format adresu e-mail.");

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Ten email istnieje w naszej bazie.");
                    }
                });
            RuleFor(x => x.PhoneNumber)
                .Custom((value, context) =>
                {
                    var phoneInUse = dbContext.Users.Any(u => u.PhoneNumber == value);

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
            RuleFor(x => x.TypeOfAccountId).NotEmpty().WithMessage("Typ konta jest wymagany.")
                .Custom((value, context) =>
                {
                    var typeExists = dbContext.TypesOfAccounts.Any(t => t.Id == value);

                    if (!typeExists)
                    {
                        context.AddFailure("TypeOfAccountId", "Nieprawidłowy identyfikator typu konta.");
                    }
                });

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotNull() // Dodane dla wartości nullable (string?)
                .NotEmpty()
                .When(x => x.StatusOfUserId == 1 && x.TypeOfAccountId == 2)
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
            foreach(Voivodeship v in enumValues)
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
