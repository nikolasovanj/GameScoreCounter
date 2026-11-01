using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configuration;

public class MatchResultConfiguration : IEntityTypeConfiguration<MatchResult>
{
    public void Configure(EntityTypeBuilder<MatchResult> builder)
    {
        builder.ToTable("match_results");
        builder.HasKey(r => r.MatchResultId);
        builder.Property(i => i.MatchResultId).ValueGeneratedNever();


        builder.Property(r => r.IsWinner).IsRequired();
        builder.Property(r => r.TotalScore);

        builder.HasMany(r => r.ScoreComponents)
               .WithOne()
               .HasForeignKey(c => c.MatchResultId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new {r.MatchId, r.UserId}).IsUnique();

        builder.HasIndex(r => r.UserId);

        builder.HasOne<User>().WithMany().HasForeignKey(r => r.UserId);
    }
}
