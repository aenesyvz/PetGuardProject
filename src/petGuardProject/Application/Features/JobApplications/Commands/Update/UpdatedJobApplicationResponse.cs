using Core.Application.Responses;

namespace Application.Features.JobApplications.Commands.Update;

public class UpdatedJobApplicationResponse : IResponse
{
    public Guid Id { get; set; }
}
