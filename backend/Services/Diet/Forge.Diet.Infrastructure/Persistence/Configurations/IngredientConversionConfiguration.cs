using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class IngredientConversionConfiguration : IEntityTypeConfiguration<IngredientConversion>
{
    public void Configure(EntityTypeBuilder<IngredientConversion> builder)
    {
        builder.ToTable("IngredientConversions");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.TargetUnitId)
            .IsRequired();

        builder.Property(c => c.ConversionFactor)
            .HasPrecision(18, 4)
            .IsRequired();

        // Optional: define foreign key relation to UnitsOfMeasure table
        builder.HasOne<UnitOfMeasure>()
            .WithMany()
            .HasForeignKey(c => c.TargetUnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
