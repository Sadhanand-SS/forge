using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Queries.GetMeal;

public record GetMealQuery(Guid Id);

public class GetMealQueryHandler
{
    private readonly IDietDbContext _context;

    public GetMealQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<MealDto> HandleAsync(GetMealQuery query, CancellationToken cancellationToken = default)
    {
        var meal = await _context.Meals
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == query.Id, cancellationToken);

        if (meal == null)
        {
            throw new KeyNotFoundException($"Meal with ID '{query.Id}' was not found.");
        }

        return meal.ToDto();
    }
}
