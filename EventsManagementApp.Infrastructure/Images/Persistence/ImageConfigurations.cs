using EventManagementApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagementApp.Infrastructure.Images.Persistence;

public class ImageConfigurations : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.ImageUrl)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Image's URL");

        builder.Property(i => i.ImageStorageName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Image's storage name");
        
        builder.HasIndex(i => new { i.ImageUrl, i.ImageStorageName });
    }
}