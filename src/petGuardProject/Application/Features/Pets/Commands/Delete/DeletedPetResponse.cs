using Core.Application.Responses;

namespace Application.Features.Pets.Commands.Delete;

public class DeletedPetResponse : IResponse
{
    public Guid Id { get; set; }
}
