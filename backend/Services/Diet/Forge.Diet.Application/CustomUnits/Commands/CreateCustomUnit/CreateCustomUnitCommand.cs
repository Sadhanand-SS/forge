using System;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Application.CustomUnits.Commands.CreateCustomUnit;

public record CreateCustomUnitCommand(string Name, string Description);

public class CreateCustomUnitCommandHandler
{
    private readonly IDietDbContext _context;

    public CreateCustomUnitCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> HandleAsync(CreateCustomUnitCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("Unit name cannot be empty.", nameof(command.Name));
        }

        var unit = new UnitOfMeasure(Guid.NewGuid(), command.Name, command.Description, isSystem: false);
        _context.UnitsOfMeasure.Add(unit);
        await _context.SaveChangesAsync(cancellationToken);

        return unit.Id;
    }
}
