using FluentValidation;

namespace Application.Features.JobApplications.Commands.Create;

public class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
{
    public CreateJobApplicationCommandValidator()
    {
        RuleFor(c => c.PetAdId).NotEmpty();
        RuleFor(c => c.BackerId).NotEmpty();
    }
}