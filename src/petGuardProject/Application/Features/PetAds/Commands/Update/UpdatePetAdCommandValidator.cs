using FluentValidation;

namespace Application.Features.PetAds.Commands.Update;

public class UpdatePetAdCommandValidator : AbstractValidator<UpdatePetAdCommand>
{
    public UpdatePetAdCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.PetOwnerId).NotEmpty();
        RuleFor(c => c.PetId).NotEmpty();
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.DistrictId).NotEmpty();
        RuleFor(c => c.StartDate).NotEmpty();
        RuleFor(c => c.EndDate).NotEmpty();
    }
}