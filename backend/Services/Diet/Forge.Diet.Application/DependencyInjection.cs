using Microsoft.Extensions.DependencyInjection;
using Forge.Diet.Application.Ingredients.Commands.CreateIngredient;
using Forge.Diet.Application.Ingredients.Commands.UpdateIngredient;
using Forge.Diet.Application.Ingredients.Commands.DeleteIngredient;
using Forge.Diet.Application.Ingredients.Queries.GetIngredient;
using Forge.Diet.Application.Ingredients.Queries.SearchIngredients;
using Forge.Diet.Application.MealItems.Commands.CreateMealItem;
using Forge.Diet.Application.MealItems.Commands.UpdateMealItem;
using Forge.Diet.Application.MealItems.Commands.DeleteMealItem;
using Forge.Diet.Application.MealItems.Commands.AddIngredientToMealItem;
using Forge.Diet.Application.MealItems.Commands.RemoveIngredientFromMealItem;
using Forge.Diet.Application.MealItems.Queries.GetMealItem;
using Forge.Diet.Application.MealItems.Queries.GetMealItems;
using Forge.Diet.Application.CustomUnits.Commands.CreateCustomUnit;
using Forge.Diet.Application.CustomUnits.Commands.UpdateCustomUnit;
using Forge.Diet.Application.CustomUnits.Commands.DeleteCustomUnit;
using Forge.Diet.Application.CustomUnits.Queries.GetCustomUnits;

namespace Forge.Diet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Ingredient Handlers
        services.AddScoped<CreateIngredientCommandHandler>();
        services.AddScoped<UpdateIngredientCommandHandler>();
        services.AddScoped<DeleteIngredientCommandHandler>();
        services.AddScoped<GetIngredientQueryHandler>();
        services.AddScoped<SearchIngredientsQueryHandler>();

        // MealItem Handlers
        services.AddScoped<CreateMealItemCommandHandler>();
        services.AddScoped<UpdateMealItemCommandHandler>();
        services.AddScoped<DeleteMealItemCommandHandler>();
        services.AddScoped<AddIngredientToMealItemCommandHandler>();
        services.AddScoped<RemoveIngredientFromMealItemCommandHandler>();
        services.AddScoped<GetMealItemQueryHandler>();
        services.AddScoped<GetMealItemsQueryHandler>();

        // CustomUnit Handlers
        services.AddScoped<CreateCustomUnitCommandHandler>();
        services.AddScoped<UpdateCustomUnitCommandHandler>();
        services.AddScoped<DeleteCustomUnitCommandHandler>();
        services.AddScoped<GetCustomUnitsQueryHandler>();

        return services;
    }
}
