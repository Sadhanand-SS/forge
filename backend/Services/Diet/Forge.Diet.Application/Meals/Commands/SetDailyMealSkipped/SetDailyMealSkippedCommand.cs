using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.SetDailyMealSkipped;

public record SetDailyMealSkippedCommand(Guid DailyMealId, bool IsSkipped);

public class SetDailyMealSkippedCommandHandler
{
    private readonly IDietDbContext _context;

    public SetDailyMealSkippedCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(SetDailyMealSkippedCommand command, CancellationToken cancellationToken = default)
    {
        var dailyMeal = await _context.DailyMeals
            .FirstOrDefaultAsync(meal => meal.Id == command.DailyMealId, cancellationToken);

        if (dailyMeal == null)
        {
            throw new KeyNotFoundException($"Daily meal with ID '{command.DailyMealId}' was not found.");
        }

        dailyMeal.SetSkipped(command.IsSkipped);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
