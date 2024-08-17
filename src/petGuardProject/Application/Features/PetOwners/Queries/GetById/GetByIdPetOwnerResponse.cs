using Core.Application.Responses;
using Domain.Enums;

namespace Application.Features.PetOwners.Queries.GetById;

public class GetByIdPetOwnerResponse : IResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalityNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string CityName { get; set; }
    public string DistrcitName { get; set; }
    public string Address { get; set; }
    public string? ImageUrl { get; set; }
    public string PhoneNumber { get; set; }

}
