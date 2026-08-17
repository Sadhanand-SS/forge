using System;

namespace Forge.Diet.Application.DTOs;

public class NutritionBasisDto
{
    public decimal Amount { get; set; }
    public Guid UnitId { get; set; }
    public string UnitName { get; set; } = null!;
}
