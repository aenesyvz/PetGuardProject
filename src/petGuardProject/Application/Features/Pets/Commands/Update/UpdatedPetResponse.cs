using Core.Application.Responses;

namespace Application.Features.Pets.Commands.Update;

public class UpdatedPetResponse : IResponse
{
    public Guid Id { get; set; }
}
