using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.RemoveMealItemFromMeal;

public record RemoveMealItemFromMealCommand(Guid MealId, Guid MealMealItemId);

public class RemoveMealItemFromMealCommandHandler
{
    private readonly IDietDbContext _context;

    public RemoveMealItemFromMealCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(RemoveMealItemFromMealCommand command, CancellationToken cancellationToken = default)
    {
        var meal = await _context.DailyMeals
            .Include(m => m.MealItems)
            .FirstOrDefaultAsync(m => m.Id == command.MealId, cancellationToken);

        if (meal == null)
        {
            throw new KeyNotFoundException($"Meal with ID '{command.MealId}' was not found.");
        }

        var mealMealItem = meal.MealItems.FirstOrDefault(item => item.Id == command.MealMealItemId);

        if (mealMealItem == null)
        {
            throw new KeyNotFoundException($"Meal item entry with ID '{command.MealMealItemId}' was not found in meal '{command.MealId}'.");
        }

        meal.RemoveMealItem(command.MealMealItemId);
        _context.DailyMealMealItems.Remove(mealMealItem);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
