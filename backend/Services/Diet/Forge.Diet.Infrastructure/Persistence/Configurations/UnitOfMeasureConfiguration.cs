using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Diet.Domain.Entities;
using Forge.Diet.Domain.Constants;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.ToTable("UnitsOfMeasure");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.IsSystem)
            .IsRequired();

        // Seed system units
        builder.HasData(
            new UnitOfMeasure(SystemUnits.Gram, "Gram", "Standard metric gram", isSystem: true),
            new UnitOfMeasure(SystemUnits.Milliliter, "Milliliter", "Standard metric milliliter", isSystem: true),
            new UnitOfMeasure(SystemUnits.Piece, "Piece", "Individual item/serving count", isSystem: true)
        );
    }
}
