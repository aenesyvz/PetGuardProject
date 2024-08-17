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
        RuleFor(c => c.PetOwnerForRegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.PetOwnerForRegisterDto.Password).NotEmpty().MinimumLength(6);
        RuleFor(c => c.PetOwnerForRegisterDto.ConfirmPassword).NotEmpty().MinimumLength(6).Equal(c => c.PetOwnerForRegisterDto.Password);
        RuleFor(c => c.PetOwnerForRegisterDto.FirstName).NotEmpty().MinimumLength(3);
        RuleFor(c => c.PetOwnerForRegisterDto.LastName).NotEmpty().MinimumLength(3);
        RuleFor(c => c.PetOwnerForRegisterDto.Gender).NotEmpty();
        RuleFor(c => c.PetOwnerForRegisterDto.PhoneNumber).NotEmpty();
        RuleFor(c => c.PetOwnerForRegisterDto.CityId).NotEmpty();
        RuleFor(c => c.PetOwnerForRegisterDto.DistrcitId).NotEmpty();
        RuleFor(c => c.PetOwnerForRegisterDto.DateOfBirth).NotEmpty();
        RuleFor(c => c.PetOwnerForRegisterDto.Address).NotEmpty();
        RuleFor(c => c.PetOwnerForRegisterDto.NationalityNumber).NotEmpty().MinimumLength(11);
    }
}