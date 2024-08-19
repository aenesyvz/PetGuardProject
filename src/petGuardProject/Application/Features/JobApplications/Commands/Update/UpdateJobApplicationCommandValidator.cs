using FluentValidation;

namespace Application.Features.JobApplications.Commands.Update;

public class UpdateJobApplicationCommandValidator : AbstractValidator<UpdateJobApplicationCommand>
{
    public UpdateJobApplicationCommandValidator()
    {
        RuleFor(c => c.PetAdId).NotEmpty();
        RuleFor(c => c.BackerId).NotEmpty();
    }
}