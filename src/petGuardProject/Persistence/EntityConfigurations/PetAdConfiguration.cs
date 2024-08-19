using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.EntityConfigurations;

public class PetAdConfiguration : IEntityTypeConfiguration<PetAd>
{
    public void Configure(EntityTypeBuilder<PetAd> builder)
    {
        builder.ToTable("PetAds").HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("Id").IsRequired();
        builder.Property(c => c.PetOwnerId).HasColumnName("PetOwnerId");
        builder.Property(c => c.PetId).HasColumnName("PetId");
        builder.Property(c => c.AdStatus).HasColumnName("AdStatus");
        builder.Property(c => c.Description).HasColumnName("Description");
        builder.Property(c => c.CityId).HasColumnName("CityId");
        builder.Property(c => c.DistrictId).HasColumnName("DistrictId");
        builder.Property(c => c.StartDate).HasColumnName("StartDate");
        builder.Property(c => c.EndDate).HasColumnName("EndDate");
        builder.Property(c => c.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(c => c.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(c => c.DeletedDate).HasColumnName("DeletedDate");

        builder.HasOne(c => c.PetOwner);
        builder.HasOne(c => c.Pet);
        builder.HasOne(c => c.City);
        builder.HasOne(c => c.District);

        builder.HasQueryFilter(c => !c.DeletedDate.HasValue);
    }
}