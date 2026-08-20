using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Forge.Diet.Application.Ingredients.Commands.CreateIngredient;
using Forge.Diet.Application.Ingredients.Commands.UpdateIngredient;
using Forge.Diet.Application.Ingredients.Commands.DeleteIngredient;
using Forge.Diet.Application.Ingredients.Queries.GetIngredient;
using Forge.Diet.Application.Ingredients.Queries.SearchIngredients;
using Forge.Diet.Application.Ingredients.Queries.CalculateIngredientNutrition;
using Forge.Diet.Application.MealItems.Commands.AddIngredientToMealItem;
using Forge.Diet.Application.MealItems.Commands.CreateMealItem;
using Forge.Diet.Application.MealItems.Commands.DeleteMealItem;
using Forge.Diet.Application.MealItems.Commands.RemoveIngredientFromMealItem;
using Forge.Diet.Application.MealItems.Commands.UpdateMealIngredient;
using Forge.Diet.Application.MealItems.Commands.UpdateMealItem;
using Forge.Diet.Application.MealItems.Queries.GetMealItem;
using Forge.Diet.Application.MealItems.Queries.GetMealItems;
using Forge.Diet.Application.Meals.Commands.AddMealItemToMeal;
using Forge.Diet.Application.Meals.Commands.CreateMeal;
using Forge.Diet.Application.Meals.Commands.DeleteMeal;
using Forge.Diet.Application.Meals.Commands.RemoveMealItemFromMeal;
using Forge.Diet.Application.Meals.Commands.SetDailyMealSkipped;
using Forge.Diet.Application.Meals.Commands.UpdateMeal;
using Forge.Diet.Application.Meals.Commands.UpdateDailyMealItemIngredients;
using Forge.Diet.Application.Meals.Commands.UpdateDailyMealItemPackedMode;
using Forge.Diet.Application.Meals.Commands.UpdateMealMealItemServings;
using Forge.Diet.Application.Meals.Queries.GetDailyMealSummary;
using Forge.Diet.Application.Meals.Queries.GetMeal;
using Forge.Diet.Application.Meals.Queries.GetMeals;
using Forge.Diet.Application.Meals.Queries.GetMealsByDate;
using Forge.Diet.Application.CustomUnits.Commands.CreateCustomUnit;
using Forge.Diet.Application.CustomUnits.Commands.UpdateCustomUnit;
using Forge.Diet.Application.CustomUnits.Commands.DeleteCustomUnit;
using Forge.Diet.Application.CustomUnits.Queries.GetCustomUnits;
using Forge.Diet.Application.DailyGoals.Commands.UpsertDailyGoal;
using Forge.Diet.Application.DailyGoals.Queries.GetDailyGoalRange;

namespace Forge.Diet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Ingredient Handlers
        services.AddScoped<CreateIngredientCommandHandler>();
        services.AddScoped<UpdateIngredientCommandHandler>();
        services.AddScoped<DeleteIngredientCommandHandler>();
        services.AddScoped<GetIngredientQueryHandler>();
        services.AddScoped<SearchIngredientsQueryHandler>();
        services.AddScoped<CalculateIngredientNutritionQueryHandler>();

        // MealItem Handlers
        services.AddScoped<CreateMealItemCommandHandler>();
        services.AddScoped<UpdateMealItemCommandHandler>();
        services.AddScoped<DeleteMealItemCommandHandler>();
        services.AddScoped<AddIngredientToMealItemCommandHandler>();
        services.AddScoped<RemoveIngredientFromMealItemCommandHandler>();
        services.AddScoped<UpdateMealIngredientCommandHandler>();
        services.AddScoped<GetMealItemQueryHandler>();
        services.AddScoped<GetMealItemsQueryHandler>();

        // Meal Handlers
        services.AddScoped<CreateMealCommandHandler>();
        services.AddScoped<UpdateMealCommandHandler>();
        services.AddScoped<DeleteMealCommandHandler>();
        services.AddScoped<AddMealItemToMealCommandHandler>();
        services.AddScoped<UpdateMealMealItemServingsCommandHandler>();
        services.AddScoped<RemoveMealItemFromMealCommandHandler>();
        services.AddScoped<SetDailyMealSkippedCommandHandler>();
        services.AddScoped<UpdateDailyMealItemIngredientsCommandHandler>();
        services.AddScoped<UpdateDailyMealItemPackedModeCommandHandler>();
        services.AddScoped<GetMealQueryHandler>();
        services.AddScoped<GetMealsQueryHandler>();
        services.AddScoped<GetMealsByDateQueryHandler>();
        services.AddScoped<GetDailyMealSummaryQueryHandler>();

        // CustomUnit Handlers
        services.AddScoped<CreateCustomUnitCommandHandler>();
        services.AddScoped<UpdateCustomUnitCommandHandler>();
        services.AddScoped<DeleteCustomUnitCommandHandler>();
        services.AddScoped<GetCustomUnitsQueryHandler>();

        // DailyGoal Handlers
        services.AddScoped<UpsertDailyGoalCommandHandler>();
        services.AddScoped<GetDailyGoalRangeQueryHandler>();

        return services;
    }
}
