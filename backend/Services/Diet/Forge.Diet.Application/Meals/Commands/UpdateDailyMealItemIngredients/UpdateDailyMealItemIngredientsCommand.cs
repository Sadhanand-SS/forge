using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.UpdateDailyMealItemIngredients;

public record UpdateDailyMealItemIngredientsCommand(
    Guid MealId,
    Guid MealMealItemId,
    List<UpdateDailyMealItemIngredientItemDto> Ingredients);

public record UpdateDailyMealItemIngredientItemDto(Guid Id, decimal Quantity, Guid UnitId);

public class UpdateDailyMealItemIngredientsCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateDailyMealItemIngredientsCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateDailyMealItemIngredientsCommand command, CancellationToken cancellationToken = default)
    {
        var mealMealItem = await _context.DailyMealMealItems
            .Include(item => item.Ingredients)
            .FirstOrDefaultAsync(item => item.Id == command.MealMealItemId && item.DailyMealId == command.MealId, cancellationToken);

        if (mealMealItem == null)
        {
            throw new KeyNotFoundException($"Meal item entry with ID '{command.MealMealItemId}' was not found in meal '{command.MealId}'.");
        }

        foreach (var ingredientDto in command.Ingredients)
        {
            var ingredient = mealMealItem.Ingredients.FirstOrDefault(i => i.Id == ingredientDto.Id);
            if (ingredient != null)
            {
                ingredient.UpdateQuantityAndUnit(ingredientDto.Quantity, ingredientDto.UnitId);
            }
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
