using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Queries.GetDailyMealSummary;

public record GetDailyMealSummaryQuery(DateOnly Date);

public class GetDailyMealSummaryQueryHandler
{
    private readonly IDietDbContext _context;

    public GetDailyMealSummaryQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<DailyMealSummaryDto> HandleAsync(GetDailyMealSummaryQuery query, CancellationToken cancellationToken = default)
    {
        await DailyMealsService.EnsureDailyMealsForDateAsync(_context, query.Date, cancellationToken);

        var meals = await _context.DailyMeals
            .Where(m => m.Date == query.Date)
            .Include(m => m.Meal)
            .Include(m => m.MealItems)
                .ThenInclude(item => item.MealItem)
                    .ThenInclude(mealItem => mealItem.Ingredients)
                        .ThenInclude(ingredient => ingredient.Ingredient)
                            .ThenInclude(ingredient => ingredient.Conversions)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return meals.ToDailySummaryDto(query.Date);
    }
}
