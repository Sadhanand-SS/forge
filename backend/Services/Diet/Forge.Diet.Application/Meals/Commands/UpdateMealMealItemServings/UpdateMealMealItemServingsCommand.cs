using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.UpdateMealMealItemServings;

public record UpdateMealMealItemServingsCommand(Guid MealId, Guid MealMealItemId, decimal Servings);

public class UpdateMealMealItemServingsCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateMealMealItemServingsCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateMealMealItemServingsCommand command, CancellationToken cancellationToken = default)
    {
        var mealMealItem = await _context.DailyMealMealItems
            .FirstOrDefaultAsync(item => item.Id == command.MealMealItemId && item.DailyMealId == command.MealId, cancellationToken);

        if (mealMealItem == null)
        {
            throw new KeyNotFoundException($"Meal item entry with ID '{command.MealMealItemId}' was not found in meal '{command.MealId}'.");
        }

        mealMealItem.UpdateServings(command.Servings);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
