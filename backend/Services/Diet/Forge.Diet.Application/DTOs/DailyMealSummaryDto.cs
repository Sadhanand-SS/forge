using System;
using System.Collections.Generic;

namespace Forge.Diet.Application.DTOs;

public class DailyMealSummaryDto
{
    public DateOnly Date { get; set; }
    public int MealCount { get; set; }
    public int SkippedMealCount { get; set; }
    public List<DailyMealDto> Meals { get; set; } = new();
    public NutritionDto TotalNutrition { get; set; } = null!;
    public DailyGoalDto? DailyGoal { get; set; }
}
