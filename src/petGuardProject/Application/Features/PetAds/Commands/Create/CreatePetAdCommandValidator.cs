using FluentValidation;

namespace Application.Features.PetAds.Commands.Create;

public class CreatePetAdCommandValidator : AbstractValidator<CreatePetAdCommand>
{
    public CreatePetAdCommandValidator()
    {
        RuleFor(c => c.PetOwnerId).NotEmpty();
        RuleFor(c => c.PetId).NotEmpty();
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty().MinimumLength(3);
        RuleFor(c => c.Description).NotEmpty().MinimumLength(3);
        RuleFor(c => c.DistrictId).NotEmpty();
        RuleFor(c => c.StartDate).NotEmpty();
        RuleFor(c => c.EndDate).NotEmpty();
    }
}