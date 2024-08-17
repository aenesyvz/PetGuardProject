using FluentValidation;

namespace Application.Features.Pets.Commands.Update;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
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