using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Districts.Commands.Create;


public class CreateDistrictCommandValidator : AbstractValidator<District>
{
    public CreateDistrictCommandValidator()
    {
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MinimumLength(3);
    }
}
