using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;

namespace Forge.Diet.Application.MealItems.Commands.UpdateMealItem;

public record UpdateMealItemCommand(Guid Id, string Name, string Description);

public class UpdateMealItemCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateMealItemCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateMealItemCommand command, CancellationToken cancellationToken = default)
    {
        var mealItem = await _context.MealItems.FindAsync(new object[] { command.Id }, cancellationToken);
        if (mealItem == null)
        {
            throw new KeyNotFoundException($"Meal item with ID '{command.Id}' was not found.");
        }

        mealItem.Update(command.Name, command.Description);

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
