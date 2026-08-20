using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.UpdateDailyMealItemPackedMode;

public record UpdateDailyMealItemPackedModeCommand(
    Guid DailyMealId,
    Guid DailyMealMealItemId,
    bool IsPacked,
    decimal? TotalCookedWeight,
    decimal? PackedWeight);

public class UpdateDailyMealItemPackedModeCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateDailyMealItemPackedModeCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateDailyMealItemPackedModeCommand command, CancellationToken cancellationToken = default)
    {
        var item = await _context.DailyMealMealItems
            .FirstOrDefaultAsync(
                i => i.Id == command.DailyMealMealItemId && i.DailyMealId == command.DailyMealId,
                CancellationToken.None);

        if (item == null)
        {
            throw new KeyNotFoundException(
                $"Meal item entry '{command.DailyMealMealItemId}' was not found in meal '{command.DailyMealId}'.");
        }

        // Domain entity validates all packed constraints (>0, packed <= cooked, etc.)
        item.SetPackedMode(command.IsPacked, command.TotalCookedWeight, command.PackedWeight);

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
