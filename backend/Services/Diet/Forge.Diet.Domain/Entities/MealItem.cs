using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Diet.Domain.ValueObjects;

namespace Forge.Diet.Domain.Entities;

public class MealItem
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public ICollection<MealIngredient> Ingredients { get; private set; }

    private MealItem()
    {
        Name = null!;
        Description = null!;
        Ingredients = new List<MealIngredient>();
    }

    public MealItem(Guid id, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Meal item name cannot be empty.", nameof(name));
        }

        Id = id;
        Name = name;
        Description = description ?? string.Empty;
        Ingredients = new List<MealIngredient>();
    }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Meal item name cannot be empty.", nameof(name));
        }

        Name = name;
        Description = description ?? string.Empty;
    }

    public MealIngredient AddIngredient(Ingredient ingredient, decimal quantity, Guid unitId)
    {
        if (ingredient == null)
        {
            throw new ArgumentNullException(nameof(ingredient));
        }

        if (!ingredient.IsCompatibleUnit(unitId))
        {
            throw new InvalidOperationException($"Unit with ID '{unitId}' is not compatible with Ingredient '{ingredient.Name}' basis unit '{ingredient.NutritionBasis.UnitId}'.");
        }

        var mealIngredient = new MealIngredient(Guid.NewGuid(), ingredient, quantity, unitId);
        Ingredients.Add(mealIngredient);
        return mealIngredient;
    }

    public void RemoveIngredient(Guid ingredientId)
    {
        var ingredient = Ingredients.FirstOrDefault(i => i.Id == ingredientId);
        if (ingredient != null)
        {
            Ingredients.Remove(ingredient);
        }
    }

    public Nutrition GetTotalNutrition()
    {
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