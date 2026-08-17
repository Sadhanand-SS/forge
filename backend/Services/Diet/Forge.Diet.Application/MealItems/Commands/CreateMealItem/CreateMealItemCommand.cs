using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Application.MealItems.Commands.CreateMealItem;

public record CreateMealItemCommand(
    string Name,
    string Description,
    List<CreateMealIngredientDetails> Ingredients
);

public record CreateMealIngredientDetails(
    Guid IngredientId,
    decimal Quantity,
    Guid UnitId
);

public class CreateMealItemCommandHandler
{
    private readonly IDietDbContext _context;

    public CreateMealItemCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> HandleAsync(CreateMealItemCommand command, CancellationToken cancellationToken = default)
    {
        var mealItem = new MealItem(
            Guid.NewGuid(),
            command.Name,
            command.Description
        );

        if (command.Ingredients != null)
        {
            foreach (var itemDetails in command.Ingredients)
            {
                var ingredient = await _context.Ingredients
                    .Include(i => i.Conversions)
                    .FirstOrDefaultAsync(i => i.Id == itemDetails.IngredientId, cancellationToken);
                    
                if (ingredient == null)
                {
                    throw new KeyNotFoundException($"Ingredient with ID '{itemDetails.IngredientId}' was not found.");
                }

                mealItem.AddIngredient(ingredient, itemDetails.Quantity, itemDetails.UnitId);
            }
        }

        _context.MealItems.Add(mealItem);
        await _context.SaveChangesAsync(cancellationToken);

        return mealItem.Id;
    }
}
