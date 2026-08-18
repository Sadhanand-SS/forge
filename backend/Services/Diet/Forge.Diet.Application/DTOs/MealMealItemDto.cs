using System;

namespace Forge.Diet.Application.DTOs;

public class MealMealItemDto
{
    public Guid Id { get; set; }
    public Guid MealItemId { get; set; }
    public string MealItemName { get; set; } = null!;
    public decimal Servings { get; set; }
    public NutritionDto TotalNutrition { get; set; } = null!;
}
