using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.EntityConfigurations;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.ToTable("Districts").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("Id").IsRequired();
        builder.Property(d => d.CityId).HasColumnName("CityId").IsRequired();
        builder.Property(d => d.Name).HasColumnName("Name");

        builder.HasOne(d => d.City);

        builder.HasQueryFilter(d => !d.DeletedDate.HasValue);
    }
}