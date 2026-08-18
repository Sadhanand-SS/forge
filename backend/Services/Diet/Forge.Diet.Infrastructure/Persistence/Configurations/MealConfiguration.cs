using Forge.Diet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.ToTable("Meals");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(m => m.Time)
            .IsRequired();

        builder.Property(m => m.IsSystem)
            .IsRequired();

        builder.HasMany(m => m.DailyMeals)
            .WithOne(item => item.Meal)
            .HasForeignKey(item => item.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.Name)
            .IsUnique();
    }
}
