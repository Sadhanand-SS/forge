using System;
using System.Collections.Generic;

namespace Forge.Diet.Application.DTOs;

public class DailyMealDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public Guid MealId { get; set; }
    public string MealName { get; set; } = null!;
    public TimeOnly Time { get; set; }
    public bool IsSystem { get; set; }
    public bool IsSkipped { get; set; }
    public List<MealMealItemDto> MealItems { get; set; } = new();
    public NutritionDto TotalNutrition { get; set; } = null!;
}
