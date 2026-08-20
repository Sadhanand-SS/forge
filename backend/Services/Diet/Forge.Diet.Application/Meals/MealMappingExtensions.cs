using System.Collections.Generic;
using System.Linq;
using Forge.Diet.Application.DTOs;
using Forge.Diet.Domain.Entities;
using Forge.Diet.Domain.ValueObjects;

namespace Forge.Diet.Application.Meals;

internal static class MealMappingExtensions
{
    public static MealDto ToDto(this Meal meal)
    {
        return new MealDto
        {
            Id = meal.Id,
            Name = meal.Name,
            Time = meal.Time,
            IsSystem = meal.IsSystem
        };
    }

    public static DailyMealDto ToDto(this DailyMeal meal, Dictionary<Guid, string>? unitNames = null)
    {
        var totalNutrition = meal.GetTotalNutrition();
        var units = unitNames ?? new Dictionary<Guid, string>();

        return new DailyMealDto
        {
            Id = meal.Id,
            Date = meal.Date,
            MealId = meal.MealId,
            MealName = meal.Meal.Name,
            Time = meal.Meal.Time,
            IsSystem = meal.Meal.IsSystem,
            IsSkipped = meal.IsSkipped,
            MealItems = meal.MealItems.Select(item =>
            {
                var itemNutrition = item.GetTotalNutrition();
                var multiplier = item.GetNutritionMultiplier();

                decimal? portionPercent = null;
                if (item.IsPacked && item.TotalCookedWeight.HasValue && item.TotalCookedWeight.Value > 0 && item.PackedWeight.HasValue)
                {
                    portionPercent = (item.PackedWeight.Value / item.TotalCookedWeight.Value) * 100m;
                }

                return new MealMealItemDto
                {
                    Id = item.Id,
                    MealItemId = item.MealItemId,
                    MealItemName = item.MealItem.Name,
                    Servings = item.Servings,
                    IsPacked = item.IsPacked,
                    TotalCookedWeight = item.TotalCookedWeight,
                    PackedWeight = item.PackedWeight,
                    TotalIngredientWeight = item.GetTotalIngredientWeight(),
                    PortionPercent = portionPercent,
                    TotalNutrition = itemNutrition.Scale(multiplier).ToDto(),
                    Ingredients = item.Ingredients.Select(ing =>
                    {
                        var compatibleUnits = new List<UnitOfMeasureDto>();

                        var baseUnitId = ing.Ingredient.NutritionBasis.UnitId;
                        var baseUnitName = units.TryGetValue(baseUnitId, out var bName) ? bName : "Unknown";
                        compatibleUnits.Add(new UnitOfMeasureDto
                        {
                            Id = baseUnitId,
                            Name = baseUnitName,
                            Description = string.Empty,
                            IsSystem = false
                        });

                        if (ing.Ingredient.Conversions != null)
                        {
                            foreach (var conv in ing.Ingredient.Conversions)
                            {
                                var targetName = units.TryGetValue(conv.TargetUnitId, out var tName) ? tName : "Unknown";
                                compatibleUnits.Add(new UnitOfMeasureDto
                                {
                                    Id = conv.TargetUnitId,
                                    Name = targetName,
                                    Description = string.Empty,
                                    IsSystem = false
                                });
                            }
                        }

                        return new DailyMealMealItemIngredientDto
                        {
                            Id = ing.Id,
                            IngredientId = ing.IngredientId,
                            IngredientName = ing.Ingredient.Name,
                            Quantity = ing.Quantity,
                            UnitId = ing.UnitId,
                            UnitName = units.TryGetValue(ing.UnitId, out var uName) ? uName : "Unknown",
                            CompatibleUnits = compatibleUnits
                        };
                    }).ToList()
                };
            }).ToList(),
            TotalNutrition = totalNutrition.ToDto()
        };
    }

    public static DailyMealSummaryDto ToDailySummaryDto(this IEnumerable<DailyMeal> meals, DateOnly date, Dictionary<Guid, string>? unitNames = null)
    {
        var mealList = meals.OrderBy(meal => meal.Meal.Time).ThenBy(meal => meal.Meal.Name).ToList();
        decimal calories = 0;
        decimal protein = 0;
        decimal carbohydrates = 0;
        decimal fat = 0;
        decimal fiber = 0;

        foreach (var meal in mealList)
        {
            var nutrition = meal.GetTotalNutrition();
            calories += nutrition.Calories;
            protein += nutrition.Protein;
            carbohydrates += nutrition.Carbohydrates;
            fat += nutrition.Fat;
            fiber += nutrition.Fiber;
        }

        return new DailyMealSummaryDto
        {
            Date = date,
            MealCount = mealList.Count,
            SkippedMealCount = mealList.Count(meal => meal.IsSkipped),
            Meals = mealList.Select(meal => meal.ToDto(unitNames)).ToList(),
            TotalNutrition = Nutrition.Create(calories, protein, carbohydrates, fat, fiber).ToDto()
        };
    }

    public static NutritionDto ToDto(this Nutrition nutrition)
    {
        return new NutritionDto
        {
            Calories = nutrition.Calories,
            Protein = nutrition.Protein,
            Carbohydrates = nutrition.Carbohydrates,
            Fat = nutrition.Fat,
            Fiber = nutrition.Fiber
        };
    }

    public static Nutrition Scale(this Nutrition nutrition, decimal multiplier)
    {
        return Nutrition.Create(
            nutrition.Calories * multiplier,
            nutrition.Protein * multiplier,
            nutrition.Carbohydrates * multiplier,
            nutrition.Fat * multiplier,
            nutrition.Fiber * multiplier);
    }
}
