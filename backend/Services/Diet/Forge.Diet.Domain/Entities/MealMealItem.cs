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

    public ICollection<DailyMealMealItemIngredient> Ingredients { get; private set; }

    private DailyMealMealItem()
    {
        DailyMeal = null!;
        MealItem = null!;
        Ingredients = new List<DailyMealMealItemIngredient>();
    }

    public DailyMealMealItem(Guid id, MealItem mealItem, decimal servings)
    {
        if (servings <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be greater than zero.");
        }

        Id = id;
        DailyMeal = null!;
        MealItem = mealItem ?? throw new ArgumentNullException(nameof(mealItem));
        MealItemId = mealItem.Id;
        Servings = servings;
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
}

