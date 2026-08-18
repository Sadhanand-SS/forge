using Forge.Diet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class MealMealItemConfiguration : IEntityTypeConfiguration<DailyMealMealItem>
{
    public void Configure(EntityTypeBuilder<DailyMealMealItem> builder)
    {
        builder.ToTable("DailyMealMealItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Servings)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.HasOne(item => item.DailyMeal)
            .WithMany(meal => meal.MealItems)
            .HasForeignKey(item => item.DailyMealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.MealItem)
            .WithMany()
            .HasForeignKey(item => item.MealItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
