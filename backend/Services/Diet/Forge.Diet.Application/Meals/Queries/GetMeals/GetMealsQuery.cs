using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Queries.GetMeals;

public record GetMealsQuery();

public class GetMealsQueryHandler
{
    private readonly IDietDbContext _context;

    public GetMealsQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<List<MealDto>> HandleAsync(GetMealsQuery query, CancellationToken cancellationToken = default)
    {
        var meals = await _context.Meals
            .AsNoTracking()
            .OrderBy(meal => meal.Time)
            .ThenBy(meal => meal.Name)
            .ToListAsync(cancellationToken);

        return meals.Select(meal => meal.ToDto()).ToList();
    }
}
