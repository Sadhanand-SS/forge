using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;

namespace Forge.Diet.Application.CustomUnits.Queries.GetCustomUnits;

public record GetCustomUnitsQuery();

public class GetCustomUnitsQueryHandler
{
    private readonly IDietDbContext _context;

    public GetCustomUnitsQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<List<UnitOfMeasureDto>> HandleAsync(GetCustomUnitsQuery query, CancellationToken cancellationToken = default)
    {
        var list = await _context.UnitsOfMeasure
            .AsNoTracking()
            .OrderByDescending(u => u.IsSystem)
            .ThenBy(u => u.Name)
            .ToListAsync(cancellationToken);

        return list.Select(u => new UnitOfMeasureDto
        {
            Id = u.Id,
            Name = u.Name,
            Description = u.Description,
            IsSystem = u.IsSystem
        }).ToList();
    }
}
