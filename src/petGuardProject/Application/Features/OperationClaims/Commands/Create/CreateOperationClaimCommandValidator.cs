using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Commands.Create;


public class CreateOperationClaimCommandValidator : AbstractValidator<CreateOperationClaimCommand>
{
    public CreateOperationClaimCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
    }
}
