using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Pets.Commands.Create;


public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(3);
        RuleFor(c => c.PetType).NotEmpty().IsInEnum().WithMessage("Pet type must be a valid value.");
        RuleFor(c => c.About).NotEmpty().MinimumLength(30);
        RuleFor(c => c.Gender).NotEmpty().IsInEnum().WithMessage("Pet gender type must be a valid value.");
        RuleFor(c => c.DateOfBirth).NotEmpty().Must(BeAValidDateInUtc);
        RuleFor(c => c.Vaccinate).NotEmpty();
        RuleFor(c => c.PetOwnerId).NotEmpty();
    }


    private bool BeAValidDateInUtc(DateTime dateOfBirth)
    {
        var utcDate = dateOfBirth.ToUniversalTime();
        return utcDate <= DateTime.UtcNow;
    }
}