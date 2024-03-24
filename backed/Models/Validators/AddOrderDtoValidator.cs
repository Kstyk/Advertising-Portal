using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models.Validators
{
    public class AddOrderDtoValidator : AbstractValidator<AddOrderDto>
    {
        private OferiaDbContext _dbContext;
        public AddOrderDtoValidator(OferiaDbContext dbContext)
        {
            _dbContext = dbContext;

            Voivodeship[] enumValues = (Voivodeship[])Enum.GetValues(typeof(Voivodeship));
            List<String> enums = new List<string>();
            foreach (Voivodeship v in enumValues)
            {
                enums.Add(v.ToString());
            }   

            RuleFor(dto => dto.Voivodeship)
                .Must((dto, voivodeship) => enums.Contains(voivodeship))
                .When(x => x.AddressId == null)
                .WithMessage("Niepoprawne województwo.");

            RuleFor(dto => dto.Description)
                .NotEmpty()
                .WithMessage("Opis jest wymagany.");

            RuleFor(dto => dto.Title)
                .NotEmpty()
                .WithMessage("Tytuł jest wymagany.");

            RuleFor(dto => dto.Budget)
                .GreaterThanOrEqualTo(0);

            RuleFor(dto => dto.PublicationDays)
                .GreaterThanOrEqualTo(5)
                .WithMessage("Publikacja musi trwać przynajmniej 5 dni.")
                .LessThanOrEqualTo(30)
                .WithMessage("Publikacja nie może trwać więcej niż 30 dni.")
                .NotNull()
                .WithMessage("Określenie liczby dni publikacji jest wymagane.");

            RuleFor(dto => dto.CategoryId)
                .GreaterThanOrEqualTo(0)
                .NotEmpty()
                .WithMessage("Musisz wybrać kategorię zlecenia.");
        }

    }
}
