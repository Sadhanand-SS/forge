using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Infrastructure.Persistence;

public class DietDbContext : DbContext, IDietDbContext
{
    public DietDbContext(DbContextOptions<DietDbContext> options) : base(options)
    {
    }

    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<IngredientConversion> IngredientConversions => Set<IngredientConversion>();
    public DbSet<UnitOfMeasure> UnitsOfMeasure => Set<UnitOfMeasure>();
    public DbSet<MealItem> MealItems => Set<MealItem>();
    public DbSet<MealIngredient> MealIngredients => Set<MealIngredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
