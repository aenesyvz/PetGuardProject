using Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.JobApplications.Commands.Delete;

public class DeletedJobApplicationResponse : IResponse
{
    public Guid Id { get; set; }
}
