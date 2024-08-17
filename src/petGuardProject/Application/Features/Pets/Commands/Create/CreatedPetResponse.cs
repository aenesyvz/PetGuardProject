using Core.Application.Responses;

namespace Application.Features.Pets.Commands.Create;

public class CreatedPetResponse : IResponse
{
    public Guid Id { get; set; }
}
