using Forge.Diet.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.Meals.Commands.UpdateMeal;

public record UpdateMealCommand(Guid Id, string Name, TimeOnly Time);

public class UpdateMealCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateMealCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateMealCommand command, CancellationToken cancellationToken = default)
    {
        var meal = await _context.Meals.FirstOrDefaultAsync(m => m.Id == command.Id, cancellationToken);

        if (meal == null)
        {
            throw new KeyNotFoundException($"Meal with ID '{command.Id}' was not found.");
        }

        meal.Update(command.Name, command.Time);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
