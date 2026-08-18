using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Application.Common.Interfaces;

public interface IDietDbContext
{
    DbSet<Ingredient> Ingredients { get; }
    DbSet<IngredientConversion> IngredientConversions { get; }
    DbSet<UnitOfMeasure> UnitsOfMeasure { get; }
    DbSet<MealItem> MealItems { get; }
    DbSet<MealIngredient> MealIngredients { get; }
    DbSet<Meal> Meals { get; }
    DbSet<DailyMeal> DailyMeals { get; }
    DbSet<DailyMealMealItem> DailyMealMealItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
