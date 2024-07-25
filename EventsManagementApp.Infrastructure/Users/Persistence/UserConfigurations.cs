using System.Security.Cryptography;
using System.Text;
using EventManagementApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsManagementApp.Infrastructure.Users.Persistence;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        ConfigureIndexes(builder);
        ConfigureProperties(builder);
        ConfigureData(builder);
        
        builder.ToTable("Users");
    }

    private static void ConfigureIndexes(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => new { u.FirstName, u.LastName });
    }

    private static void ConfigureProperties(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(32)
            .HasComment("User's first name");

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(32)
            .HasComment("User's last name");

        builder.Property(u => u.DateOfBirth)
            .IsRequired()
            .HasColumnType("Date")
            .HasComment("User's date of birth");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(64)
            .HasComment("User's email");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(256)
            .HasComment("User's hashed password");
    }
    
    private static void ConfigureData(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1990, 1, 1),
                PasswordHash = Encoding.UTF8.GetString(SHA256.HashData("Asteria347@"u8.ToArray())),
                Email = "asteria@gmail.com"
            }
        );
    }
}