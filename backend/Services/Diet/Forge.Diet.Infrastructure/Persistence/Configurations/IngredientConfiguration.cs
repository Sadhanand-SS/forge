using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable("Ingredients");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.Brand)
            .HasMaxLength(100);

        builder.Property(i => i.NetWeight)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.HasMany(i => i.Conversions)
            .WithOne()
            .HasForeignKey(c => c.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        // ComplexProperty mapping for NutritionBasis and Nutrition
        builder.ComplexProperty(i => i.NutritionBasis, nb =>
        {
            nb.Property(p => p.Amount)
                .HasColumnName("NutritionBasisAmount")
                .HasPrecision(18, 4)
                .IsRequired();

            nb.Property(p => p.UnitId)
                .HasColumnName("NutritionBasisUnitId")
                .IsRequired();
        });

        builder.ComplexProperty(i => i.Nutrition, n =>
        {
            n.Property(p => p.Calories)
                .HasColumnName("Calories")
                .HasPrecision(18, 4)
                .IsRequired();

            n.Property(p => p.Protein)
                .HasColumnName("Protein")
                .HasPrecision(18, 4)
                .IsRequired();

            n.Property(p => p.Carbohydrates)
                .HasColumnName("Carbohydrates")
                .HasPrecision(18, 4)
                .IsRequired();

            n.Property(p => p.Fat)
                .HasColumnName("Fat")
                .HasPrecision(18, 4)
                .IsRequired();

            n.Property(p => p.Fiber)
                .HasColumnName("Fiber")
                .HasPrecision(18, 4)
                .IsRequired();
        });
    }
}
