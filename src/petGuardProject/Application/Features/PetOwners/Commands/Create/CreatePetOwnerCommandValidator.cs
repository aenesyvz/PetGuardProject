using Application.Features.PetOwners.Commands.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetOwners.Commands.Create;


public class CreatePetOwnerCommandValidator : AbstractValidator<CreatePetOwnerCommand>
{
    public CreatePetOwnerCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(6);
        RuleFor(c => c.ConfirmPassword).NotEmpty().MinimumLength(6).Equal(c => c.Password);
        RuleFor(c => c.FirstName).NotEmpty().MinimumLength(3);
        RuleFor(c => c.LastName).NotEmpty().MinimumLength(3);
        RuleFor(c => c.Gender).NotEmpty();
        RuleFor(c => c.PhoneNumber).NotEmpty();
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.DistrcitId).NotEmpty();
        RuleFor(c => c.DateOfBirth).NotEmpty();
        RuleFor(c => c.Address).NotEmpty();
        RuleFor(c => c.NationalityNumber).NotEmpty().MinimumLength(11);
    }
}