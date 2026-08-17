using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class MealIngredientConfiguration : IEntityTypeConfiguration<MealIngredient>
{
    public void Configure(EntityTypeBuilder<MealIngredient> builder)
    {
        builder.ToTable("MealIngredients");

        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.Quantity)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(mi => mi.UnitId)
            .IsRequired();

        builder.HasOne(mi => mi.Ingredient)
            .WithMany()
            .HasForeignKey(mi => mi.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional: define foreign key relation to UnitsOfMeasure table
        builder.HasOne<UnitOfMeasure>()
            .WithMany()
            .HasForeignKey(mi => mi.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
