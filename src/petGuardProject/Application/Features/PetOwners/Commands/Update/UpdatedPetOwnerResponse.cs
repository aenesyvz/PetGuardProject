using Core.Application.Responses;

namespace Application.Features.PetOwners.Commands.Update;

public class UpdatedPetOwnerResponse : IResponse
{
    public Guid Id { get; set; }
}
