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
        // NOTE: CancellationToken.None is intentional on all DB calls below.
        // The HTTP request CancellationToken is cancelled whenever the browser refreshes
        // or navigates away. Propagating it to Npgsql causes OperationCanceledException
        // to bubble up through the entire stack. Since these are fast read queries against
        // a remote DB (Supabase), we let them complete regardless of client cancellation.
        var ct = CancellationToken.None;

        await DailyMealsService.EnsureDailyMealsForDateAsync(_context, query.Date, ct);

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
            .ToListAsync(ct);

        var dailyGoal = await _context.DailyGoals
            .Where(dg => dg.Date <= query.Date)
            .OrderByDescending(dg => dg.Date)
            .FirstOrDefaultAsync(ct);

        var goalDto = dailyGoal != null ? new DailyGoalDto
        {
            Id = dailyGoal.Id,
            Date = dailyGoal.Date,
            Calories = dailyGoal.Calories,
            Protein = dailyGoal.Protein,
            Carbohydrates = dailyGoal.Carbohydrates,
            Fat = dailyGoal.Fat,
            Fiber = dailyGoal.Fiber
        } : new DailyGoalDto
        {
            Id = Guid.Empty,
            Date = query.Date,
            Calories = 2000,
            Protein = 150,
            Carbohydrates = 200,
            Fat = 70,
            Fiber = 25
        };

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, ct);

        var summary = meals.ToDailySummaryDto(query.Date, units);
        summary.DailyGoal = goalDto;
        return summary;
    }
}
