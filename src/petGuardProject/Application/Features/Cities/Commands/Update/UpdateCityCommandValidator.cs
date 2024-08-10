using Domain.Entities;
using FluentValidation;

namespace Application.Features.Cities.Commands.Update;

public class UpdateCityCommandValidator : AbstractValidator<City>
{

    public UpdateCityCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MinimumLength(3);
        RuleFor(c => c.PlateCode).NotEmpty().GreaterThan(0).LessThanOrEqualTo(81);
    }
}
