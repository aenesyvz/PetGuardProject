using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.EntityConfigurations;

public class BackerConfiguration : IEntityTypeConfiguration<Backer>
{
    public void Configure(EntityTypeBuilder<Backer> builder)
    {
        builder.ToTable("Backers").HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("Id").IsRequired();
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
        builder.Property(c => c.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(c => c.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(c => c.DeletedDate).HasColumnName("DeletedDate");

        builder.HasOne(c => c.User);

        builder.HasQueryFilter(c => !c.DeletedDate.HasValue);
    }
}