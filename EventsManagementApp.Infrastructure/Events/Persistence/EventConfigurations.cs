using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagementApp.Infrastructure.Events.Persistence;

public class EventConfigurations : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        ConfigureIndexes(builder);
        ConfigureProperties(builder);
        
        builder.HasMany(e => e.Images)
            .WithOne(i => i.Event)
            .HasForeignKey(i => i.EventId);
        
        builder.ToTable("Events");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Name) 
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Event's name");
        
        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .HasComment("Event's description");

        builder.Property(e => e.StartDate)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ")
            .HasComment("Event's start date");
        
        builder.Property(e => e.EndDate)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ")
            .HasComment("Event's end date");
        
        builder.Property(e => e.Location)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Event's location");

        builder.Property(e => e.Category)
            .IsRequired()
            .HasConversion<string>()
            .HasSentinel(-1)
            .HasDefaultValue(CategoryEnum.Other)
            .HasComment("Event's category");
        
        builder.Property(e => e.Capacity)
            .IsRequired()
            .HasComment("Event's capacity");
    }

    private static void ConfigureIndexes(EntityTypeBuilder<Event> builder)
    {
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Category);
        builder.HasIndex(e => e.Location);
        builder.HasIndex(e => new { e.StartDate, e.EndDate });
    }
}