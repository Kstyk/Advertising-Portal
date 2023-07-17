using FluentValidation;
using System;
using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models.Validators
{
    public class AreaOfWorkDtoValidator : AbstractValidator<AreaOfWorkDto>
    {
        public AreaOfWorkDtoValidator()
        {
            Voivodeship[] enumValues = (Voivodeship[])Enum.GetValues(typeof(Voivodeship));
            List<String> enums = new List<string>();
            foreach (Voivodeship v in enumValues)
            {
                enums.Add(v.ToString());
            }

           RuleFor(dto => dto.Voivodeship)
                .Must((dto, voivodeship) => enums.Contains(voivodeship) || (voivodeship == null && dto.WholeCountry != null))
                .WithMessage("Niepoprawne województwo.");
        }
    }
}
