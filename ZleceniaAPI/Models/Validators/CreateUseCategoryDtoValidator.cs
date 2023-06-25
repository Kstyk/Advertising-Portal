using FluentValidation;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models.Validators
{

    public class CreateUseCategoryDtoValidator : AbstractValidator<CreateUserCategoryDto>
    {
        public CreateUseCategoryDtoValidator(OferiaDbContext context)
        {
            RuleFor(dto => dto.Categories).NotEmpty();

            Voivodeship[] enumValues = (Voivodeship[])Enum.GetValues(typeof(Voivodeship));
            List<String> enums = new List<string>();
            foreach (Voivodeship v in enumValues)
            {
                enums.Add(v.ToString());
            }

            RuleFor(dto => dto.Voivodeship)
                .Must((dto, voivodeship) => (!string.IsNullOrEmpty(voivodeship) && string.IsNullOrEmpty(dto.WholeCountry) && enums.Contains(voivodeship))
                                          || (string.IsNullOrEmpty(voivodeship) && !string.IsNullOrEmpty(dto.WholeCountry)))
                .WithMessage("Musisz wybrać konkretne województwo lub cały kraj.");

            RuleFor(dto => dto.Categories)
                .NotEmpty()
                .WithMessage("Musisz wybrać przynajmniej jedną kategorię.")
                .Must((categories) => categories != null && categories.Count() > 0)
                .WithMessage("Musisz wybrać przynajmniej jedną kategorię.");

        }
    }
   
}
