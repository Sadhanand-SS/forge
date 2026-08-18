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

    public static DailyMealDto ToDto(this DailyMeal meal)
    {
        var totalNutrition = meal.GetTotalNutrition();

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
                var itemNutrition = item.MealItem.GetTotalNutrition();

                return new MealMealItemDto
                {
                    Id = item.Id,
                    MealItemId = item.MealItemId,
                    MealItemName = item.MealItem.Name,
                    Servings = item.Servings,
                    TotalNutrition = itemNutrition.Scale(item.Servings).ToDto()
                };
            }).ToList(),
            TotalNutrition = totalNutrition.ToDto()
        };
    }

    public static DailyMealSummaryDto ToDailySummaryDto(this IEnumerable<DailyMeal> meals, DateOnly date)
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
            Meals = mealList.Select(meal => meal.ToDto()).ToList(),
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
