using Core.Application.Responses;

namespace Application.Features.JobApplications.Commands.Create;

public class CreatedJobApplicationResponse : IResponse
{
    public Guid Id { get; set; }
}
