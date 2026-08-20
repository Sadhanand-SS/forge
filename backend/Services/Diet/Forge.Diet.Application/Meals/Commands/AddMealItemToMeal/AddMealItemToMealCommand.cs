using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.AddMealItemToMeal;

public record AddMealItemToMealCommand(Guid MealId, Guid MealItemId, decimal Servings);

public class AddMealItemToMealCommandHandler
{
    private readonly IDietDbContext _context;

    public AddMealItemToMealCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> HandleAsync(AddMealItemToMealCommand command, CancellationToken cancellationToken = default)
    {
        var meal = await _context.DailyMeals
            .Include(m => m.MealItems)
            .FirstOrDefaultAsync(m => m.Id == command.MealId, cancellationToken);

        if (meal == null)
        {
            throw new KeyNotFoundException($"Meal with ID '{command.MealId}' was not found.");
        }

        var mealItem = await _context.MealItems
            .Include(mi => mi.Ingredients)
                .ThenInclude(mi => mi.Ingredient)
            .FirstOrDefaultAsync(item => item.Id == command.MealItemId, cancellationToken);

        if (mealItem == null)
        {
            throw new KeyNotFoundException($"Meal item with ID '{command.MealItemId}' was not found.");
        }

        var mealMealItem = meal.AddMealItem(mealItem, command.Servings);
        _context.DailyMealMealItems.Add(mealMealItem);
        await _context.SaveChangesAsync(CancellationToken.None);

        return mealMealItem.Id;
    }
}
