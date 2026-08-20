using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class DailyMealMealItemIngredientConfiguration : IEntityTypeConfiguration<DailyMealMealItemIngredient>
{
    public void Configure(EntityTypeBuilder<DailyMealMealItemIngredient> builder)
    {
        builder.ToTable("DailyMealMealItemIngredients");

        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.Quantity)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(mi => mi.UnitId)
            .IsRequired();

        builder.HasOne(mi => mi.DailyMealMealItem)
            .WithMany(m => m.Ingredients)
            .HasForeignKey(mi => mi.DailyMealMealItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mi => mi.Ingredient)
            .WithMany()
            .HasForeignKey(mi => mi.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<UnitOfMeasure>()
            .WithMany()
            .HasForeignKey(mi => mi.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
