using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Districts.Commands.Update;

public class UpdateDistrictCommandValidator : AbstractValidator<District>
{
    public UpdateDistrictCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MinimumLength(3);
    }
}
