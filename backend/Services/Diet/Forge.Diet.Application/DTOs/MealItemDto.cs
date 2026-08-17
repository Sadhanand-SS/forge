using System;
using System.Collections.Generic;

namespace Forge.Diet.Application.DTOs;

public class MealItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<MealIngredientDto> Ingredients { get; set; } = new();
    public NutritionDto TotalNutrition { get; set; } = null!;
}
