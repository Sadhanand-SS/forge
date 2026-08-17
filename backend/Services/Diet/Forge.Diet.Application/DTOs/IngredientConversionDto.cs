using System;

namespace Forge.Diet.Application.DTOs;

public class IngredientConversionDto
{
    public Guid Id { get; set; }
    public Guid TargetUnitId { get; set; }
    public string TargetUnitName { get; set; } = null!;
    public decimal ConversionFactor { get; set; }
}
