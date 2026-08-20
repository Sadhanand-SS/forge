using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.Ingredients.Commands.CreateIngredient;
using Forge.Diet.Domain.Entities;
using Forge.Diet.Domain.ValueObjects;

namespace Forge.Diet.Application.Ingredients.Commands.UpdateIngredient;

public record UpdateIngredientCommand(
    Guid Id,
    string Name,
    string? Brand,
    decimal NetWeight,
    decimal NutritionBasisAmount,
    Guid NutritionBasisUnitId,
    decimal Calories,
    decimal Protein,
    decimal Carbohydrates,
    decimal Fat,
    decimal Fiber,
    List<CreateIngredientConversionDetails>? Conversions = null
);

public class UpdateIngredientCommandHandler
{
    private readonly IDietDbContext _context;

    public UpdateIngredientCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateIngredientCommand command, CancellationToken cancellationToken = default)
    {
        var ingredient = await _context.Ingredients
            .Include(i => i.Conversions)
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (ingredient == null)
        {
            throw new KeyNotFoundException($"Ingredient with ID '{command.Id}' was not found.");
        }

        var nutritionBasis = NutritionBasis.Create(command.NutritionBasisAmount, command.NutritionBasisUnitId);
        var nutrition = Nutrition.Create(command.Calories, command.Protein, command.Carbohydrates, command.Fat, command.Fiber);

        ingredient.Update(command.Name, command.Brand, command.NetWeight, nutritionBasis, nutrition);

        ingredient.ClearConversions();
        if (command.Conversions != null)
        {
            foreach (var conv in command.Conversions)
            {
                ingredient.AddConversion(conv.TargetUnitId, conv.ConversionFactor);
            }
        }

        foreach (var conv in ingredient.Conversions)
        {
            _context.IngredientConversions.Add(conv);
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
