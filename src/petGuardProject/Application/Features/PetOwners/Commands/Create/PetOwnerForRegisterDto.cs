using Core.Application.Dtos;
using Domain.Enums;

namespace Application.Features.PetOwners.Commands.Create;

public class PetOwnerForRegisterDto : IDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalityNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrcitId { get; set; }
    public string Address { get; set; }
    public string? ImageUrl { get; set; }
    public string PhoneNumber { get; set; }
}