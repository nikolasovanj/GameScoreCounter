using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configuration;

public class ScoreComponentConfiguration : IEntityTypeConfiguration<ScoreComponent>
{
    public void Configure(EntityTypeBuilder<ScoreComponent> builder)
    {
        builder.ToTable("score_components");
        builder.HasKey(c => c.ComponetId);

        builder.Property(c => c.Label)
               .HasMaxLength(100)
               .IsRequired();
        builder.Property(c => c.Points).IsRequired();

        builder.HasIndex(c => new {c.MatchResultId, c.Order}).IsUnique();
    }
}
