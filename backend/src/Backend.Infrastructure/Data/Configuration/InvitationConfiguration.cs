using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configuration;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("invitations");
        
        builder.HasKey(i => i.InvitationId);
        builder.Property(i => i.InvitationId).ValueGeneratedNever();

        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(i => i.PlayerId)
               .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(i => new { i.ScheduleId, i.PlayerId}).IsUnique();
        builder.HasIndex(i => new { i.PlayerId, i.Status});
    }
}