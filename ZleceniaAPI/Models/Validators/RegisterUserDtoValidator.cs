using FluentValidation;
using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(OferiaDbContext dbContext)
        {
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Ten email istnieje w naszej bazie");
                    }
                });

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.Voivodeship).NotEmpty();
            RuleFor(x => x.BuildingNumber).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.StatusOfUserId).NotEmpty()
                .Custom((value, context) =>
                {
                    var statusExists = dbContext.StatusOfUsers.Any(s => s.Id == value);

                    if (!statusExists)
                    {
                        context.AddFailure("StatusOfUserId", "Nieprawidłowy identyfikator statusu użytkownika");
                    }
                });
            RuleFor(x => x.TypeOfAccountId).NotEmpty()
                .Custom((value, context) =>
                {
                    var typeExists = dbContext.TypesOfAccounts.Any(t => t.Id == value);

                    if (!typeExists)
                    {
                        context.AddFailure("TypeOfAccountId", "Nieprawidłowy identyfikator typu konta");
                    }
                });

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotNull() // Dodane dla wartości nullable (string?)
                .NotEmpty()
                .When(x => x.TypeOfAccountId == 1 && x.StatusOfUserId == 1)
                .WithMessage("Pole opisu firmy nie może być puste");

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
        }
    }
}
