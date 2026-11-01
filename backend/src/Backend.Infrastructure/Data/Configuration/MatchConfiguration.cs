using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configuration;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("matches");
        builder.HasKey(m => m.MatchId);
        builder.Property(i => i.MatchId).ValueGeneratedNever();


        builder.Property(m => m.Game)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(m => m.PlayedAt).IsRequired();

        builder.HasMany(m => m.Results)
               .WithOne()
               .HasForeignKey(r => r.MatchId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new {m.Game, m.PlayedAt});
    }
}
