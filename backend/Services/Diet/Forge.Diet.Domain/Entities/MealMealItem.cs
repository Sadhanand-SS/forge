using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Diet.Domain.ValueObjects;

namespace Forge.Diet.Domain.Entities;

public class DailyMealMealItem
{
    public Guid Id { get; private set; }

    public Guid DailyMealId { get; private set; }

    public DailyMeal DailyMeal { get; private set; }

    public Guid MealItemId { get; private set; }

    public MealItem MealItem { get; private set; }

    public decimal Servings { get; private set; }

    // --- Packed Mode ---

    /// <summary>When true, nutrition is calculated from PackedWeight/TotalCookedWeight rather than Servings.</summary>
    public bool IsPacked { get; private set; }

    /// <summary>Total weight (g) of the entire cooked batch. Required when IsPacked is true.</summary>
    public decimal? TotalCookedWeight { get; private set; }

    /// <summary>Weight (g) the user actually packed/consumed from the batch. Required when IsPacked is true.</summary>
    public decimal? PackedWeight { get; private set; }

    public ICollection<DailyMealMealItemIngredient> Ingredients { get; private set; }

    private DailyMealMealItem()
    {
        DailyMeal = null!;
        MealItem = null!;
        Ingredients = new List<DailyMealMealItemIngredient>();
    }

    public DailyMealMealItem(Guid id, MealItem mealItem, decimal servings,
        bool isPacked = false, decimal? totalCookedWeight = null, decimal? packedWeight = null)
    {
        if (servings <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be greater than zero.");
        }

        ValidatePackedFields(isPacked, totalCookedWeight, packedWeight);

        Id = id;
        DailyMeal = null!;
        MealItem = mealItem ?? throw new ArgumentNullException(nameof(mealItem));
        MealItemId = mealItem.Id;
        Servings = servings;
        IsPacked = isPacked;
        TotalCookedWeight = isPacked ? totalCookedWeight : null;
        PackedWeight = isPacked ? packedWeight : null;
        Ingredients = new List<DailyMealMealItemIngredient>();

        if (mealItem.Ingredients != null)
        {
            foreach (var ingredient in mealItem.Ingredients)
            {
                Ingredients.Add(new DailyMealMealItemIngredient(
                    Guid.NewGuid(),
                    Id,
                    ingredient.IngredientId,
                    ingredient.Ingredient,
                    ingredient.Quantity,
                    ingredient.UnitId
                ));
            }
        }
    }

    public void UpdateServings(decimal servings)
    {
        if (servings <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be greater than zero.");
        }

        Servings = servings;
    }

    public void SetPackedMode(bool isPacked, decimal? totalCookedWeight, decimal? packedWeight)
    {
        ValidatePackedFields(isPacked, totalCookedWeight, packedWeight);
        IsPacked = isPacked;
        TotalCookedWeight = isPacked ? totalCookedWeight : null;
        PackedWeight = isPacked ? packedWeight : null;
    }

    /// <summary>
    /// Returns the effective nutrition multiplier for this logged meal item entry.
    /// When packed, this is PackedWeight / TotalCookedWeight (portion ratio).
    /// When not packed, this is Servings.
    /// DailyMeal.GetTotalNutrition() uses this to multiply item nutrition.
    /// </summary>
    public decimal GetNutritionMultiplier()
    {
        if (IsPacked && TotalCookedWeight.HasValue && TotalCookedWeight.Value > 0 && PackedWeight.HasValue)
        {
            return PackedWeight.Value / TotalCookedWeight.Value;
        }
        return Servings;
    }

    /// <summary>
    /// Returns total raw ingredient weight in the default unit (g) from the cloned ingredient list.
    /// Used as a display-only value in the Packed UI.
    /// </summary>
    public decimal GetTotalIngredientWeight()
    {
        if (Ingredients == null || !Ingredients.Any()) return 0;
        decimal total = 0;
        foreach (var ing in Ingredients)
        {
            if (ing.Ingredient == null) continue;
            var factor = ing.Ingredient.GetConversionFactor(ing.UnitId);
            total += ing.Quantity * factor;
        }
        return total;
    }

    public Nutrition GetTotalNutrition()
    {
        if (Ingredients == null || !Ingredients.Any())
        {
            return MealItem?.GetTotalNutrition() ?? Nutrition.Create(0, 0, 0, 0, 0);
        }

        decimal calories = 0;
        decimal protein = 0;
        decimal carbohydrates = 0;
        decimal fat = 0;
        decimal fiber = 0;

        foreach (var ingredient in Ingredients)
        {
            if (ingredient.Ingredient == null)
            {
                continue;
            }

            if (!ingredient.Ingredient.IsCompatibleUnit(ingredient.UnitId))
            {
                continue;
            }

            var conversionFactor = ingredient.Ingredient.GetConversionFactor(ingredient.UnitId);
            var quantityInDefaultUnit = ingredient.Quantity * conversionFactor;
            var ratio = quantityInDefaultUnit / ingredient.Ingredient.NutritionBasis.Amount;
            
            calories += ingredient.Ingredient.Nutrition.Calories * ratio;
            protein += ingredient.Ingredient.Nutrition.Protein * ratio;
            carbohydrates += ingredient.Ingredient.Nutrition.Carbohydrates * ratio;
            fat += ingredient.Ingredient.Nutrition.Fat * ratio;
            fiber += ingredient.Ingredient.Nutrition.Fiber * ratio;
        }

        return Nutrition.Create(calories, protein, carbohydrates, fat, fiber);
    }

    private static void ValidatePackedFields(bool isPacked, decimal? totalCookedWeight, decimal? packedWeight)
    {
        if (!isPacked) return;

        if (!totalCookedWeight.HasValue || totalCookedWeight.Value <= 0)
        {
            throw new ArgumentException("Total cooked weight must be greater than zero when packed mode is enabled.");
        }

        if (!packedWeight.HasValue || packedWeight.Value <= 0)
        {
            throw new ArgumentException("Packed weight must be greater than zero when packed mode is enabled.");
        }

        if (packedWeight.Value > totalCookedWeight.Value)
        {
            throw new ArgumentException("Packed weight cannot be greater than total cooked weight.");
        }
    }
}
