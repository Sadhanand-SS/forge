using System;

namespace Forge.Diet.Application.DTOs;

public class MealIngredientDto
{
    public Guid Id { get; set; }
    public Guid IngredientId { get; set; }
    public string IngredientName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public Guid UnitId { get; set; }
    public string UnitName { get; set; } = null!;
}
