using Forge.Diet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class DailyMealConfiguration : IEntityTypeConfiguration<DailyMeal>
{
    public void Configure(EntityTypeBuilder<DailyMeal> builder)
    {
        builder.ToTable("DailyMeals");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Date)
            .IsRequired();

        builder.Property(m => m.IsSkipped)
            .IsRequired();

        builder.HasOne(m => m.Meal)
            .WithMany(meal => meal.DailyMeals)
            .HasForeignKey(m => m.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.Date, m.MealId })
            .IsUnique();
    }
}
