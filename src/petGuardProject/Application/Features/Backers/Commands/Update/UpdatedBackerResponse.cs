using Core.Application.Responses;

namespace Application.Features.Backers.Commands.Update;

public class UpdatedBackerResponse : IResponse
{
    public Guid Id { get; set; }
}
