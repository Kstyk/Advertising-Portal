using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
                .WithMessage("Niepoprawne województwo.");

            RuleFor(dto => dto.Budget)
                .GreaterThanOrEqualTo(0);

            RuleFor(dto => dto.PublicationDays)
                .GreaterThanOrEqualTo(5)
                .LessThanOrEqualTo(30)
                .WithMessage("Publikacja może trwać od 5 do 30 dni");

            RuleFor(dto => dto.CategoryId)
                .GreaterThanOrEqualTo(0);
        }

    }
}
