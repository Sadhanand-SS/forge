using System;

namespace Forge.Diet.Domain.ValueObjects;

public sealed record NutritionBasis
{
    public decimal Amount { get; }
    public Guid UnitId { get; }

    private NutritionBasis(decimal amount, Guid unitId)
    {
        Amount = amount;
        UnitId = unitId;
    }

    public static NutritionBasis Create(
        decimal amount,
        Guid unitId)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Nutrition basis amount must be greater than zero.");
        }

        if (unitId == Guid.Empty)
        {
            throw new ArgumentException("Unit ID cannot be empty.", nameof(unitId));
        }

        return new NutritionBasis(amount, unitId);
    }
}