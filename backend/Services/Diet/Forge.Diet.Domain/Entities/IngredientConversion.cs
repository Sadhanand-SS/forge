using System;

namespace Forge.Diet.Domain.Entities;

public class IngredientConversion
{
    public Guid Id { get; private set; }

    public Guid IngredientId { get; private set; }

    public Guid TargetUnitId { get; private set; }

    public decimal ConversionFactor { get; private set; }

    private IngredientConversion()
    {
    }

    public IngredientConversion(Guid id, Guid targetUnitId, decimal conversionFactor)
    {
        if (targetUnitId == Guid.Empty)
        {
            throw new ArgumentException("Target unit ID cannot be empty.", nameof(targetUnitId));
        }

        if (conversionFactor <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(conversionFactor), "Conversion factor must be greater than zero.");
        }

        Id = id;
        TargetUnitId = targetUnitId;
        ConversionFactor = conversionFactor;
    }
}
