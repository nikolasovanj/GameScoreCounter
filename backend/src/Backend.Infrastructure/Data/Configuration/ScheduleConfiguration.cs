using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configuration;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("schedules");

        builder.HasKey(s => s.ScheduleId);
        builder.Property(s => s.ScheduleId).ValueGeneratedNever();

        builder.Property(s => s.Game).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(s => s.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(s => s.Location).HasMaxLength(200);
        builder.Property(s => s.MatchTime).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasMany(s => s.Invitations)
               .WithOne()
               .HasForeignKey(i => i.ScheduleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(s => s.OrganizerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Match>()
               .WithMany()
               .HasForeignKey(s => s.MatchId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.OrganizerId, s.MatchTime});
        builder.HasIndex(s => s.MatchTime);
    }
}