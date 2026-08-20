using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Domain.Entities;
using Forge.Diet.Domain.ValueObjects;

namespace Forge.Diet.Application.Ingredients.Commands.CreateIngredient;

public record CreateIngredientCommand(
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

public record CreateIngredientConversionDetails(
    Guid TargetUnitId,
    decimal ConversionFactor
);

public class CreateIngredientCommandHandler
{
    private readonly IDietDbContext _context;

    public CreateIngredientCommandHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> HandleAsync(CreateIngredientCommand command, CancellationToken cancellationToken = default)
    {
        var nutritionBasis = NutritionBasis.Create(command.NutritionBasisAmount, command.NutritionBasisUnitId);
        var nutrition = Nutrition.Create(command.Calories, command.Protein, command.Carbohydrates, command.Fat, command.Fiber);

        var ingredient = new Ingredient(
            Guid.NewGuid(),
            command.Name,
            command.Brand,
            command.NetWeight,
            nutritionBasis,
            nutrition
        );

        if (command.Conversions != null)
        {
            foreach (var conv in command.Conversions)
            {
                ingredient.AddConversion(conv.TargetUnitId, conv.ConversionFactor);
            }
        }

        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync(CancellationToken.None);

        return ingredient.Id;
    }
}
