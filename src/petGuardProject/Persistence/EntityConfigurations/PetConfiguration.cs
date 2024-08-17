using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.EntityConfigurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets").HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("Id").IsRequired();
        builder.Property(c => c.PetOwnerId).HasColumnName("PetOwnerId");
        builder.Property(c => c.PetType).HasColumnName("PetType");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.About).HasColumnName("About");
        builder.Property(c => c.Gender).HasColumnName("Gender");
        builder.Property(c => c.DateOfBirth).HasColumnName("DateOfBirth");
        builder.Property(c => c.ImageUrl).HasColumnName("ImageUrl");
        builder.Property(c => c.Vaccinate).HasColumnName("Vaccinate");
        builder.Property(c => c.Weight).HasColumnName("Weight");
        builder.Property(c => c.Height).HasColumnName("Height");

        builder.Property(c => c.PetOwner);

        builder.HasQueryFilter(c => !c.DeletedDate.HasValue);
    }
}