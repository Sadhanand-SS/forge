using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;

namespace Forge.Diet.Application.CustomUnits.Commands.UpdateCustomUnit;

public record UpdateCustomUnitCommand(Guid Id, string Name, string Description);

public class UpdateCustomUnitCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateCustomUnitCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateCustomUnitCommand command, CancellationToken cancellationToken = default)
    {
        var unit = await _context.UnitsOfMeasure.FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);
        if (unit == null)
        {
            throw new KeyNotFoundException($"Unit of measure with ID '{command.Id}' was not found.");
        }

        unit.Update(command.Name, command.Description);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
