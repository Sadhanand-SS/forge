using System;

namespace Forge.Diet.Application.DTOs;

public class IngredientNutritionCalculationDto
{
    public Guid IngredientId { get; set; }
    public string IngredientName { get; set; } = null!;
    public Guid UnitId { get; set; }
    public string UnitName { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal AmountInBasisUnit { get; set; }
    public NutritionBasisDto NutritionBasis { get; set; } = null!;
    public NutritionDto Nutrition { get; set; } = null!;
}
