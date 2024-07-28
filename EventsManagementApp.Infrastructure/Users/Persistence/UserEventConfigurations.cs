using EventManagementApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagementApp.Infrastructure.Users.Persistence;

public class UserEventConfigurations : IEntityTypeConfiguration<UserEvent>
{
    public void Configure(EntityTypeBuilder<UserEvent> builder)
    {
        builder.HasKey(ue => new { ue.UserId, ue.EventId });
        
        ConfigureProperties(builder);

        builder.HasOne(ue => ue.User)
            .WithMany(u => u.UserEvents)
            .HasForeignKey(ue => ue.UserId);
        
        builder.HasOne(ue => ue.Event)
            .WithMany(e => e.UserEvents)
            .HasForeignKey(ue => ue.EventId);
        
        builder.ToTable("UserEvents");
    }

    private static void ConfigureProperties(EntityTypeBuilder<UserEvent> builder)
    {
        builder.Property(ue => ue.UserId)
            .IsRequired()
            .HasComment("User's ID");
        
        builder.Property(ue => ue.EventId)
            .IsRequired()
            .HasComment("Event's ID");
        
        builder.Property(ue => ue.RegistrationDate)
            .IsRequired()
            .HasColumnType("TIMESTAMPTZ")
            .HasDefaultValueSql("NOW()")
            .HasComment("Registration date");
    }
}