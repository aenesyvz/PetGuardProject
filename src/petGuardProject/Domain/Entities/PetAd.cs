using Core.Persistence.Repositories;
using Domain.Enums;

namespace Domain.Entities;

public class PetAd: Entity<Guid>
{
    public Guid PetOwnerId { get; set; }
    public Guid PetId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AdStatus AdStatus { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrictId { get; set; }


    public virtual PetOwner PetOwner { get; set; }
    public virtual Pet Pet { get; set; }
    public virtual City City { get; set; }
    public virtual District District { get; set; }

    public PetAd()
    {
        
    }

    public PetAd(Guid id,Guid petOwnerId,Guid petId,string description,DateTime startDate,DateTime endDate):this()
    {
        Id = id;
        PetOwnerId = petOwnerId;
        PetId = petId;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }
}