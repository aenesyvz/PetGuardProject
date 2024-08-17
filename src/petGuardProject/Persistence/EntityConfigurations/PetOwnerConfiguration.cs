using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.EntityConfigurations;

public class PetOwnerConfiguration : IEntityTypeConfiguration<PetOwner>
{
    public void Configure(EntityTypeBuilder<PetOwner> builder)
    {
        builder.ToTable("PetOwners").HasKey(c => c.Id);

        builder.Property(c => c.UserId).HasColumnName("UserId");
        builder.Property(c => c.FirstName).HasColumnName("FirstName");
        builder.Property(c => c.LastName).HasColumnName("LastName");
        builder.Property(c => c.NationalityNumber).HasColumnName("NationalityNumber");
        builder.Property(c => c.DateOfBirth).HasColumnName("DateOfBirth");
        builder.Property(c => c.Gender).HasColumnName("Gender");
        builder.Property(c => c.CityId).HasColumnName("CityId");
        builder.Property(c => c.DistrcitId).HasColumnName("DistrictId");
        builder.Property(c => c.Address).HasColumnName("Address");
        builder.Property(c => c.ImageUrl).HasColumnName("ImageUrl");
        builder.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber");

        builder.HasOne(c => c.User);
        builder.HasMany(c => c.Pets);

        builder.HasQueryFilter(c => !c.DeletedDate.HasValue);
    }
}