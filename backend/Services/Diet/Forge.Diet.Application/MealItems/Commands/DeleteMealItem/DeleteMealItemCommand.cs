using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;

namespace Forge.Diet.Application.MealItems.Commands.DeleteMealItem;

public record DeleteMealItemCommand(Guid Id);

public class DeleteMealItemCommandHandler
{
    private readonly IDietDbContext _context;

    public DeleteMealItemCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(DeleteMealItemCommand command, CancellationToken cancellationToken = default)
    {
        var mealItem = await _context.MealItems.FindAsync(new object[] { command.Id }, cancellationToken);
        if (mealItem == null)
        {
            throw new KeyNotFoundException($"Meal item with ID '{command.Id}' was not found.");
        }

        _context.MealItems.Remove(mealItem);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
