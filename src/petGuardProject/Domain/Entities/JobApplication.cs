using Core.Persistence.Repositories;

namespace Domain.Entities;

public class JobApplication : Entity<Guid>
{
    public Guid PetAdId { get; set; }
    public Guid BackerId { get; set; }

    public virtual PetAd PetAd { get; set; }
    public virtual Backer Backer { get; set; }


    public JobApplication()
    {
        
    }

    public JobApplication(Guid id, Guid petAdId, Guid backerId) : this()
    {
        Id = id;
        PetAdId = petAdId;
        BackerId = backerId;
    }
}