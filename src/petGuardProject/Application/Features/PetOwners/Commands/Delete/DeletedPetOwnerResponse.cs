using Core.Application.Responses;

namespace Application.Features.PetOwners.Commands.Delete;

public class DeletedPetOwnerResponse : IResponse
{
    public Guid Id { get; set; }
}
