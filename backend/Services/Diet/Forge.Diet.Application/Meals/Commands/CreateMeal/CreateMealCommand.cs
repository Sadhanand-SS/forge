using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Domain.Entities;

namespace Forge.Diet.Application.Meals.Commands.CreateMeal;

public record CreateMealCommand(string Name, TimeOnly Time);

public class CreateMealCommandHandler
{
    private readonly IDietDbContext _context;

    public CreateMealCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> HandleAsync(CreateMealCommand command, CancellationToken cancellationToken = default)
    {
        var meal = new Meal(Guid.NewGuid(), command.Name, command.Time, isSystem: false);
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync(CancellationToken.None);

        return meal.Id;
    }
}
