using Application.Services.BackerService;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Backers.Commands.Create;

public class CreateBackerCommandValidator:AbstractValidator<CreateBackerCommand>
{
    public CreateBackerCommandValidator()
    {
        RuleFor(c => c.BackerForRegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.BackerForRegisterDto.FirstName).NotEmpty().MinimumLength(3);
        RuleFor(c => c.BackerForRegisterDto.LastName).NotEmpty().MinimumLength(3);
        RuleFor(c => c.BackerForRegisterDto.Gender).NotEmpty();
        RuleFor(c => c.BackerForRegisterDto.PhoneNumber).NotEmpty();
        RuleFor(c => c.BackerForRegisterDto.CityId).NotEmpty();
        RuleFor(c => c.BackerForRegisterDto.DistrcitId).NotEmpty();
        RuleFor(c => c.BackerForRegisterDto.DateOfBirth).NotEmpty();
        RuleFor(c => c.BackerForRegisterDto.Address).NotEmpty();
        RuleFor(c => c.BackerForRegisterDto.NationalityNumber).NotEmpty().MinimumLength(11);
    }
}
