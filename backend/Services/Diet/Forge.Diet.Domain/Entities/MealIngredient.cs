using System;

namespace Forge.Diet.Domain.Entities;

public class MealIngredient
{
    public Guid Id { get; private set; }

    public Guid IngredientId { get; private set; }

    public Ingredient Ingredient { get; private set; }

    public decimal Quantity { get; private set; }

    public Guid UnitId { get; private set; }

    private MealIngredient()
    {
        Ingredient = null!;
    }

    public MealIngredient(Guid id, Ingredient ingredient, decimal quantity, Guid unitId)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        if (unitId == Guid.Empty)
        {
            throw new ArgumentException("Unit ID cannot be empty.", nameof(unitId));
        }

        Id = id;
        Ingredient = ingredient ?? throw new ArgumentNullException(nameof(ingredient));
        IngredientId = ingredient.Id;
        Quantity = quantity;
        UnitId = unitId;
    }

    public void UpdateQuantityAndUnit(decimal quantity, Guid unitId)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        if (unitId == Guid.Empty)
        {
            throw new ArgumentException("Unit ID cannot be empty.", nameof(unitId));
        }

        Quantity = quantity;
        UnitId = unitId;
    }
}
