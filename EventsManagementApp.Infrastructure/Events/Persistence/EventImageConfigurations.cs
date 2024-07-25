using EventManagementApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagementApp.Infrastructure.Events.Persistence;

public class EventImageConfigurations : IEntityTypeConfiguration<EventImage>
{
    public void Configure(EntityTypeBuilder<EventImage> builder)
    {
        builder.HasKey(ei => new { ei.EventId, ei.ImageId });
        
        builder.Property(ei => ei.EventId)
            .IsRequired()
            .HasComment("Event's ID");
        
        builder.Property(ei => ei.ImageId)
            .IsRequired()
            .HasComment("Image's ID");
        
        builder.HasOne(ei => ei.Event)
            .WithMany(e => e.EventImages)
            .HasForeignKey(ei => ei.EventId);
        
        builder.HasOne(ei => ei.Image)
            .WithMany(i => i.EventImages)
            .HasForeignKey(ei => ei.ImageId);
        
        builder.ToTable("EventImages");
    }
}