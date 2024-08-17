using Core.Persistence.Repositories;
using Domain.Enums;

namespace Domain.Entities;

public class Pet : Entity<Guid>
{
    public string Name { get; set; }
    public PetType PetType { get; set; }
    public string About { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public bool Vaccinate { get; set; }
    public string Weight { get; set; }
    public string Height { get; set; }
    public Guid PetOwnerId { get; set; }

    public virtual PetOwner PetOwner { get; set; }

    public Pet()
    {
        
    }

    public Pet(Guid id,string name, PetType animalType, string about, Gender gender, DateTime dateOfBirth, string ımageUrl, bool vaccinate, string weight, string height, Guid petOwnerId) : this()
    {
        Id = id;
        Name = name;
        PetType = animalType;
        About = about;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        ImageUrl = ımageUrl;
        Vaccinate = vaccinate;
        Weight = weight;
        Height = height;
        PetOwnerId = petOwnerId;
    }
}