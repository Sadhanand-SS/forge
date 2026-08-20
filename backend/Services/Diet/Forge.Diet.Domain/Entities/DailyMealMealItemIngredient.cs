using System;

namespace Forge.Diet.Domain.Entities;

public class DailyMealMealItemIngredient
{
    public Guid Id { get; private set; }

    public Guid DailyMealMealItemId { get; private set; }

    public DailyMealMealItem? DailyMealMealItem { get; private set; }

    public Guid IngredientId { get; private set; }

    public Ingredient Ingredient { get; private set; }

    public decimal Quantity { get; private set; }

    public Guid UnitId { get; private set; }

    private DailyMealMealItemIngredient()
    {
        Ingredient = null!;
    }

    public DailyMealMealItemIngredient(Guid id, Guid dailyMealMealItemId, Guid ingredientId, Ingredient ingredient, decimal quantity, Guid unitId)
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
        DailyMealMealItemId = dailyMealMealItemId;
        IngredientId = ingredientId;
        Ingredient = ingredient ?? throw new ArgumentNullException(nameof(ingredient));
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
