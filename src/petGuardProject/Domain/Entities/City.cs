using Core.Persistence.Repositories;

namespace Domain.Entities;

public class City : Entity<Guid>
{
    public string Name { get; set; }
    public int PlateCode { get; set; }


    public virtual ICollection<District> Districts { get; set; }


    public City()
    {
        Districts = new HashSet<District>();   
    }

    public City(Guid id,string name,int plateCode):this()
    {
        Id = id;
        Name = name;
        PlateCode = plateCode;
    }
}
