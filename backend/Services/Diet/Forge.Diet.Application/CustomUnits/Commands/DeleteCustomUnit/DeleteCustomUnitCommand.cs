using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;

namespace Forge.Diet.Application.CustomUnits.Commands.DeleteCustomUnit;

public record DeleteCustomUnitCommand(Guid Id);

public class DeleteCustomUnitCommandHandler
{
    private readonly IDietDbContext _context;

    public DeleteCustomUnitCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(DeleteCustomUnitCommand command, CancellationToken cancellationToken = default)
    {
        var unit = await _context.UnitsOfMeasure.FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);
        if (unit == null)
        {
            throw new KeyNotFoundException($"Unit of measure with ID '{command.Id}' was not found.");
        }

        if (unit.IsSystem)
        {
            throw new InvalidOperationException("Cannot delete system-defined units of measure.");
        }

        // Check references in Ingredients basis unit
        var inUseInIngredients = await _context.Ingredients
            .AnyAsync(i => i.NutritionBasis.UnitId == command.Id, cancellationToken);

        if (inUseInIngredients)
        {
            throw new InvalidOperationException($"Unit of measure '{unit.Name}' is currently used as the basis unit for one or more ingredients and cannot be deleted.");
        }

        // Check references in Conversions target unit
        var inUseInConversions = await _context.IngredientConversions
            .AnyAsync(c => c.TargetUnitId == command.Id, cancellationToken);

        if (inUseInConversions)
        {
            throw new InvalidOperationException($"Unit of measure '{unit.Name}' is currently used in conversions for one or more ingredients and cannot be deleted.");
        }

        // Check references in MealIngredients unit
        var inUseInMealIngredients = await _context.MealIngredients
            .AnyAsync(mi => mi.UnitId == command.Id, cancellationToken);

        if (inUseInMealIngredients)
        {
            throw new InvalidOperationException($"Unit of measure '{unit.Name}' is currently used in one or more meal recipes and cannot be deleted.");
        }

        _context.UnitsOfMeasure.Remove(unit);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
