using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals;

internal static class DailyMealsService
{
    public static async Task EnsureDailyMealsForDateAsync(
        IDietDbContext context,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        // NOTE: CancellationToken.None is intentional on all DB calls.
        // Seeding must complete atomically — a partial insert caused by a browser
        // refresh cancelling the token leaves the day without meal slots.
        var ct = CancellationToken.None;

        var mealDefinitions = await context.Meals
            .AsNoTracking()
            .OrderBy(meal => meal.Time)
            .ThenBy(meal => meal.Name)
            .ToListAsync(ct);

        if (mealDefinitions.Count == 0)
        {
            return;
        }

        var existingMealIds = await context.DailyMeals
            .Where(meal => meal.Date == date)
            .Select(meal => meal.MealId)
            .ToListAsync(ct);

        var dailyMealsToAdd = mealDefinitions
            .Where(meal => !existingMealIds.Contains(meal.Id))
            .Select(meal => new DailyMeal(Guid.NewGuid(), date, meal.Id))  // FK-only: avoids EF re-tracking the AsNoTracking Meal entity
            .ToList();

        if (dailyMealsToAdd.Count == 0)
        {
            return;
        }

        await context.DailyMeals.AddRangeAsync(dailyMealsToAdd, ct);

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException?.Message.Contains("23505") == true ||
                  ex.InnerException?.Message.Contains("duplicate key") == true)
        {
            // Race condition: another concurrent request already inserted daily meals
            // for this date. The data is already present, so this is safe to ignore.
        }
    }
}
