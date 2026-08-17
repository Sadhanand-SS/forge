using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Infrastructure.Persistence.Configurations;

public class MealItemConfiguration : IEntityTypeConfiguration<MealItem>
{
    public void Configure(EntityTypeBuilder<MealItem> builder)
    {
        builder.ToTable("MealItems");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasMany(m => m.Ingredients)
            .WithOne()
            .HasForeignKey("MealItemId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
