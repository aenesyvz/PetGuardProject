using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Features.UserOperationClaims.Commands.Update;

public class UpdateUserOperationClaimCommandValidator : AbstractValidator<UpdateUserOperationClaimCommand>
{
    public UpdateUserOperationClaimCommandValidator()
    {
        RuleFor(c => c.UserId).NotNull();
        RuleFor(c => c.OperationClaimId).GreaterThan(0);
    }
}
