using Core.Persistence.Repositories;
using Domain.Enums;

namespace Domain.Entities;

public class PetOwner : Entity<Guid>
{
    public Guid UserId { get; set; }
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

    public virtual User User { get; set; }
    public virtual ICollection<Pet> Pets { get; set; }

    public PetOwner()
    {
        Pets = new HashSet<Pet>();
    }

    public PetOwner(Guid id,Guid userId,string firstName, string lastName, string nationalityNumber, DateTime dateOfBirth,Gender gender, Guid cityId, Guid distrcitId, string address, string imageUrl, string phoneNumber) : this()
    {
        Id = id;
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        NationalityNumber = nationalityNumber;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        CityId = cityId;
        DistrcitId = distrcitId;
        Address = address;
        ImageUrl = imageUrl;
        PhoneNumber = phoneNumber;
    }
}