using FluentValidation;

namespace Application.Features.PetOwners.Commands.Update;

public class UpdatePetOwnerCommandValidator: AbstractValidator<UpdatePetOwnerCommand>
{
    public UpdatePetOwnerCommandValidator()
    {
        RuleFor(c => c.PhoneNumber).NotEmpty();
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.DistrcitId).NotEmpty();
        RuleFor(c => c.Address).NotEmpty();
    }
}