using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;

namespace Forge.Diet.Application.Ingredients.Commands.DeleteIngredient;

public record DeleteIngredientCommand(Guid Id);

public class DeleteIngredientCommandHandler
{
    private readonly IDietDbContext _context;

    public DeleteIngredientCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(DeleteIngredientCommand command, CancellationToken cancellationToken = default)
    {
        var ingredient = await _context.Ingredients.FindAsync(new object[] { command.Id }, cancellationToken);
        if (ingredient == null)
        {
            throw new KeyNotFoundException($"Ingredient with ID '{command.Id}' was not found.");
        }

        var inUse = await _context.MealIngredients.AnyAsync(mi => mi.IngredientId == command.Id, cancellationToken);
        if (inUse)
        {
            throw new InvalidOperationException($"Ingredient '{ingredient.Name}' cannot be deleted because it is currently used in one or more meal items.");
        }

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
