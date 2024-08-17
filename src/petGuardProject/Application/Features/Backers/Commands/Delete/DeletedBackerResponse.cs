using Core.Application.Responses;

namespace Application.Features.Backers.Commands.Delete;

public class DeletedBackerResponse : IResponse
{
    public Guid Id { get; set; }
}
