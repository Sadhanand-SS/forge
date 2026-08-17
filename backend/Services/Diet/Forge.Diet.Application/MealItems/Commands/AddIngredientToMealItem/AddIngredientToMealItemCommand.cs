using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;

namespace Forge.Diet.Application.MealItems.Commands.AddIngredientToMealItem;

public record AddIngredientToMealItemCommand(
    Guid MealItemId,
    Guid IngredientId,
    decimal Quantity,
    Guid UnitId
);

public class AddIngredientToMealItemCommandHandler
{
    private readonly IDietDbContext _context;

    public AddIngredientToMealItemCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> HandleAsync(AddIngredientToMealItemCommand command, CancellationToken cancellationToken = default)
    {
        var mealItem = await _context.MealItems
            .Include(m => m.Ingredients)
            .FirstOrDefaultAsync(m => m.Id == command.MealItemId, cancellationToken);

        if (mealItem == null)
        {
            throw new KeyNotFoundException($"Meal item with ID '{command.MealItemId}' was not found.");
        }

        var ingredient = await _context.Ingredients
            .Include(i => i.Conversions)
            .FirstOrDefaultAsync(i => i.Id == command.IngredientId, cancellationToken);
            
        if (ingredient == null)
        {
            throw new KeyNotFoundException($"Ingredient with ID '{command.IngredientId}' was not found.");
        }

        var mealIngredient = mealItem.AddIngredient(ingredient, command.Quantity, command.UnitId);

        _context.MealIngredients.Add(mealIngredient);

        await _context.SaveChangesAsync(cancellationToken);

        return mealIngredient.Id;
    }
}
