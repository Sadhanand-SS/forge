using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.DeleteMeal;

public record DeleteMealCommand(Guid Id);

public class DeleteMealCommandHandler
{
    private readonly IDietDbContext _context;

    public DeleteMealCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(DeleteMealCommand command, CancellationToken cancellationToken = default)
    {
        var meal = await _context.Meals.FirstOrDefaultAsync(m => m.Id == command.Id, cancellationToken);

        if (meal == null)
        {
            throw new KeyNotFoundException($"Meal with ID '{command.Id}' was not found.");
        }

        if (meal.IsSystem)
        {
            throw new InvalidOperationException("System meals cannot be deleted.");
        }

        var hasDailyPlans = await _context.DailyMeals
            .AnyAsync(dailyMeal => dailyMeal.MealId == command.Id, cancellationToken);

        if (hasDailyPlans)
        {
            throw new InvalidOperationException("This meal is already used in one or more daily plans and cannot be deleted.");
        }

        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
