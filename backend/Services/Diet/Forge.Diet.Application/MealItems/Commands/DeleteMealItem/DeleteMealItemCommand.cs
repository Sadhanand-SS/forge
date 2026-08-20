using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        var inUseInDailyMeals = await _context.DailyMealMealItems
            .AnyAsync(item => item.MealItemId == command.Id, cancellationToken);

        if (inUseInDailyMeals)
        {
            throw new InvalidOperationException($"Meal item '{mealItem.Name}' is currently used in one or more daily meals and cannot be deleted.");
        }

        _context.MealItems.Remove(mealItem);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
