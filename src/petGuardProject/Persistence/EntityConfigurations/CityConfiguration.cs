using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.EntityConfigurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities").HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("Id").IsRequired();
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.PlateCode).HasColumnName("PlateCode");

        builder.HasMany(c => c.Districts);

        builder.HasQueryFilter(c => !c.DeletedDate.HasValue);
    }
}
