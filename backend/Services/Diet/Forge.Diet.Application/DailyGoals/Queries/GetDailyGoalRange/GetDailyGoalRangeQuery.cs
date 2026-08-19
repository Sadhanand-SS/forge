using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.DailyGoals.Queries.GetDailyGoalRange;

public record GetDailyGoalRangeQuery(DateOnly StartDate, DateOnly EndDate);

public class GetDailyGoalRangeQueryHandler
{
    private readonly IDietDbContext _context;

    public GetDailyGoalRangeQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<List<DailySummaryRangeItemDto>> HandleAsync(GetDailyGoalRangeQuery query, CancellationToken cancellationToken = default)
    {
        var dailyMeals = await _context.DailyMeals
            .Where(m => m.Date >= query.StartDate && m.Date <= query.EndDate)
            .Include(m => m.MealItems)
                .ThenInclude(item => item.MealItem)
                    .ThenInclude(mealItem => mealItem.Ingredients)
                        .ThenInclude(ingredient => ingredient.Ingredient)
                            .ThenInclude(ingredient => ingredient.Conversions)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // 2. Fetch all DailyGoals up to EndDate (to find closest goal for each day in range)
        var dailyGoals = await _context.DailyGoals
            .Where(dg => dg.Date <= query.EndDate)
            .OrderBy(dg => dg.Date)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var result = new List<DailySummaryRangeItemDto>();

        // Loop through each date in the range
        for (var date = query.StartDate; date <= query.EndDate; date = date.AddDays(1))
        {
            // Calculate eaten macros for this day
            var mealsForDay = dailyMeals.Where(m => m.Date == date).ToList();
            decimal eatenCal = 0, eatenProt = 0, eatenCarb = 0, eatenFat = 0, eatenFib = 0;

            foreach (var dm in mealsForDay)
            {
                if (dm.IsSkipped) continue;

                foreach (var item in dm.MealItems)
                {
                    if (item.MealItem == null) continue;
                    var nutrition = item.MealItem.GetTotalNutrition();
                    eatenCal += nutrition.Calories * item.Servings;
                    eatenProt += nutrition.Protein * item.Servings;
                    eatenCarb += nutrition.Carbohydrates * item.Servings;
                    eatenFat += nutrition.Fat * item.Servings;
                    eatenFib += nutrition.Fiber * item.Servings;
                }
            }

            // Find closest goal for this date
            var goalForDay = dailyGoals
                .Where(dg => dg.Date <= date)
                .OrderByDescending(dg => dg.Date)
                .FirstOrDefault();

            var goalDto = goalForDay != null ? new DailyGoalDto
            {
                Id = goalForDay.Id,
                Date = goalForDay.Date,
                Calories = goalForDay.Calories,
                Protein = goalForDay.Protein,
                Carbohydrates = goalForDay.Carbohydrates,
                Fat = goalForDay.Fat,
                Fiber = goalForDay.Fiber
            } : new DailyGoalDto
            {
                Id = Guid.Empty,
                Date = date,
                Calories = 2000,
                Protein = 150,
                Carbohydrates = 200,
                Fat = 70,
                Fiber = 25
            };

            result.Add(new DailySummaryRangeItemDto
            {
                Date = date,
                EatenCalories = eatenCal,
                EatenProtein = eatenProt,
                EatenCarbohydrates = eatenCarb,
                EatenFat = eatenFat,
                EatenFiber = eatenFib,
                Goal = goalDto
            });
        }

        return result;
    }
}
