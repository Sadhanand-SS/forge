using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;

namespace Forge.Diet.Application.MealItems.Commands.RemoveIngredientFromMealItem;

public record RemoveIngredientFromMealItemCommand(Guid MealItemId, Guid MealIngredientId);

public class RemoveIngredientFromMealItemCommandHandler
{
    private readonly IDietDbContext _context;

    public RemoveIngredientFromMealItemCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(RemoveIngredientFromMealItemCommand command, CancellationToken cancellationToken = default)
    {
        var mealItem = await _context.MealItems
            .Include(m => m.Ingredients)
            .FirstOrDefaultAsync(m => m.Id == command.MealItemId, cancellationToken);

        if (mealItem == null)
        {
            throw new KeyNotFoundException($"Meal item with ID '{command.MealItemId}' was not found.");
        }

        var ingredient = mealItem.Ingredients.FirstOrDefault(i => i.Id == command.MealIngredientId);
        if (ingredient == null)
        {
            throw new KeyNotFoundException($"Meal ingredient with ID '{command.MealIngredientId}' was not found in this meal item.");
        }

        mealItem.RemoveIngredient(command.MealIngredientId);

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
