using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Queries.GetMealsByDate;

public record GetMealsByDateQuery(DateOnly Date);

public class GetMealsByDateQueryHandler
{
    private readonly IDietDbContext _context;

    public GetMealsByDateQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<List<DailyMealDto>> HandleAsync(GetMealsByDateQuery query, CancellationToken cancellationToken = default)
    {
        // NOTE: Do NOT call EnsureDailyMealsForDateAsync here.
        // Seeding is owned by GetDailySummaryQueryHandler which the frontend always
        // calls first. Calling Ensure from both endpoints in parallel causes a
        // duplicate-key race that rolls back the entire insert batch in both
        // transactions, leaving DailyMeals empty for the requested date.

        // NOTE: CancellationToken.None is intentional. See GetDailyMealSummaryQuery for reasoning.
        var ct = CancellationToken.None;

        var meals = await _context.DailyMeals
            .Where(m => m.Date == query.Date)
            .Include(m => m.Meal)
            .Include(m => m.MealItems)
                .ThenInclude(item => item.MealItem)
                    .ThenInclude(mealItem => mealItem.Ingredients)
                        .ThenInclude(ingredient => ingredient.Ingredient)
                            .ThenInclude(ingredient => ingredient.Conversions)
            .Include(m => m.MealItems)
                .ThenInclude(item => item.Ingredients)
                    .ThenInclude(ingredient => ingredient.Ingredient)
                        .ThenInclude(ingredient => ingredient.Conversions)
            .AsNoTracking()
            .OrderBy(m => m.Meal.Time)
            .ThenBy(m => m.Meal.Name)
            .ToListAsync(ct);

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, ct);

        return meals.Select(meal => meal.ToDto(units)).ToList();
    }
}
