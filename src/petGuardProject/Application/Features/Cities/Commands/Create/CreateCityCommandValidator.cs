using Domain.Entities;
using FluentValidation;

namespace Application.Features.Cities.Commands.Create;

public class CreateCityCommandValidator: AbstractValidator<City>
{
    public CreateCityCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.PlateCode).GreaterThan(0).LessThanOrEqualTo(81);
    }
}
