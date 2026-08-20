using System;
using System.Collections.Generic;

namespace Forge.Diet.Application.DTOs;

public class MealMealItemDto
{
    public Guid Id { get; set; }
    public Guid MealItemId { get; set; }
    public string MealItemName { get; set; } = null!;
    public decimal Servings { get; set; }

    // Packed mode fields
    public bool IsPacked { get; set; }
    public decimal? TotalCookedWeight { get; set; }
    public decimal? PackedWeight { get; set; }
    /// <summary>Sum of all cloned ingredient weights in the default unit (g). Display-only.</summary>
    public decimal TotalIngredientWeight { get; set; }
    /// <summary>PackedWeight / TotalCookedWeight expressed as 0-100. Null when not packed.</summary>
    public decimal? PortionPercent { get; set; }

    public NutritionDto TotalNutrition { get; set; } = null!;
    public List<DailyMealMealItemIngredientDto> Ingredients { get; set; } = new();
}
