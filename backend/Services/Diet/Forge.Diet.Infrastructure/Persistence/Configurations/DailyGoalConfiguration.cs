using Forge.Diet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class DailyGoalConfiguration : IEntityTypeConfiguration<DailyGoal>
{
    public void Configure(EntityTypeBuilder<DailyGoal> builder)
    {
        builder.ToTable("DailyGoals");

        builder.HasKey(dg => dg.Id);

        builder.Property(dg => dg.Date)
            .IsRequired();

        builder.Property(dg => dg.Calories)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(dg => dg.Protein)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(dg => dg.Carbohydrates)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(dg => dg.Fat)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(dg => dg.Fiber)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.HasIndex(dg => dg.Date)
            .IsUnique();
    }
}
