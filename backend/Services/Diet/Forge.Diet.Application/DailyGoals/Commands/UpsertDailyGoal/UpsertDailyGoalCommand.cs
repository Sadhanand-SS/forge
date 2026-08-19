using System;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Application.DailyGoals.Commands.UpsertDailyGoal;

public record UpsertDailyGoalCommand(
    DateOnly Date,
    decimal Calories,
    decimal Protein,
    decimal Carbohydrates,
    decimal Fat,
    decimal Fiber);

public class UpsertDailyGoalCommandHandler
{
    private readonly IDietDbContext _context;

    public UpsertDailyGoalCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> HandleAsync(UpsertDailyGoalCommand command, CancellationToken cancellationToken = default)
    {
        var existingGoal = await _context.DailyGoals
            .FirstOrDefaultAsync(dg => dg.Date == command.Date, cancellationToken);

        if (existingGoal != null)
        {
            existingGoal.Update(
                command.Calories,
                command.Protein,
                command.Carbohydrates,
                command.Fat,
                command.Fiber);

            await _context.SaveChangesAsync(cancellationToken);
            return existingGoal.Id;
        }

        var newGoal = new DailyGoal(
            Guid.NewGuid(),
            command.Date,
            command.Calories,
            command.Protein,
            command.Carbohydrates,
            command.Fat,
            command.Fiber);

        await _context.DailyGoals.AddAsync(newGoal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return newGoal.Id;
    }
}
