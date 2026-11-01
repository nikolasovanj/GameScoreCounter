using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");
        builder.HasKey(u => u.UserId);
        builder.Property(i => i.UserId).ValueGeneratedNever();


        builder.Property(u => u.Username)
               .HasMaxLength(20)
               .IsRequired();
        builder.Property(u => u.PasswordHash)
               .HasMaxLength(200)
               .IsRequired();
    }
}
