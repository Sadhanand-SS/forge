using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.MealItems.Commands.UpdateMealIngredient;

public record UpdateMealIngredientCommand(Guid MealItemId, Guid IngredientId, decimal Quantity, Guid UnitId);

public class UpdateMealIngredientCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateMealIngredientCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateMealIngredientCommand command, CancellationToken cancellationToken = default)
    {
        var mealItem = await _context.MealItems
            .Include(mi => mi.Ingredients)
                .ThenInclude(i => i.Ingredient)
            .FirstOrDefaultAsync(mi => mi.Id == command.MealItemId, CancellationToken.None);

        if (mealItem == null)
        {
            throw new KeyNotFoundException($"Meal item with ID '{command.MealItemId}' was not found.");
        }

        var ingredient = mealItem.Ingredients.FirstOrDefault(i => i.Id == command.IngredientId);
        if (ingredient == null)
        {
            throw new KeyNotFoundException($"Ingredient entry '{command.IngredientId}' was not found in meal item '{command.MealItemId}'.");
        }

        // Validate unit is compatible before updating
        if (!ingredient.Ingredient.IsCompatibleUnit(command.UnitId))
        {
            throw new ArgumentException($"Unit '{command.UnitId}' is not compatible with ingredient '{ingredient.Ingredient.Name}'.");
        }

        ingredient.UpdateQuantityAndUnit(command.Quantity, command.UnitId);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
