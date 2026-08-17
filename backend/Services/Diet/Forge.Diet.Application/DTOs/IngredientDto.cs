using System;
using System.Collections.Generic;

namespace Forge.Diet.Application.DTOs;

public class IngredientDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Brand { get; set; }
    public decimal NetWeight { get; set; }
    public NutritionBasisDto NutritionBasis { get; set; } = null!;
    public NutritionDto Nutrition { get; set; } = null!;
    public List<IngredientConversionDto> Conversions { get; set; } = new();
}
