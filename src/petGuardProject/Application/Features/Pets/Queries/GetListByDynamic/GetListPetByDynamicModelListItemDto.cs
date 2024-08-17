using Core.Application.Dtos;
using Domain.Enums;

namespace Application.Features.Pets.Queries.GetListByDynamic;

public class GetListPetByDynamicModelListItemDto:IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PetOwnerFirstName { get; set; }
    public string PetOwnerLastName { get; set; }
    public string PetOwnerEmail { get; set; }
    public string PetOwnerPhoneNumber { get; set; }
    public PetType PetType { get; set; }
    public string About { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public bool Vaccinate { get; set; }
    public string Weight { get; set; }
    public string Height { get; set; }
    public Guid PetOwnerId { get; set; }
}
