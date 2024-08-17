using FluentValidation;

namespace Application.Features.Backers.Commands.Update;

public class UpdateBackerCommandValidator : AbstractValidator<UpdateBackerCommand>
{
    public UpdateBackerCommandValidator()
    {
        RuleFor(c => c.PhoneNumber).NotEmpty();
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.DistrcitId).NotEmpty();
        RuleFor(c => c.Address).NotEmpty();

    }
}
