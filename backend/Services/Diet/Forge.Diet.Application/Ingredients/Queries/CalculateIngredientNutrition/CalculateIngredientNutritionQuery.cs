using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;

namespace Forge.Diet.Application.Ingredients.Queries.CalculateIngredientNutrition;

public record CalculateIngredientNutritionQuery(Guid IngredientId, Guid UnitId, decimal Amount);

public class CalculateIngredientNutritionQueryHandler
{
    private readonly IDietDbContext _context;

    public CalculateIngredientNutritionQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<IngredientNutritionCalculationDto> HandleAsync(
        CalculateIngredientNutritionQuery query,
        CancellationToken cancellationToken = default)
    {
        var ingredient = await _context.Ingredients
            .Include(i => i.Conversions)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == query.IngredientId, cancellationToken);

        if (ingredient == null)
        {
            throw new KeyNotFoundException($"Ingredient with ID '{query.IngredientId}' was not found.");
        }

        if (!ingredient.IsCompatibleUnit(query.UnitId))
        {
            throw new InvalidOperationException(
                $"Unit with ID '{query.UnitId}' is not compatible with ingredient '{ingredient.Name}'.");
        }

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, CancellationToken.None);

        if (!units.TryGetValue(query.UnitId, out var requestedUnitName))
        {
            throw new KeyNotFoundException($"Unit with ID '{query.UnitId}' was not found.");
        }

        var basisUnitName = units.TryGetValue(ingredient.NutritionBasis.UnitId, out var value)
            ? value
            : "Unknown";

        var conversionFactor = ingredient.GetConversionFactor(query.UnitId);
        var amountInBasisUnit = query.Amount * conversionFactor;
        var scale = amountInBasisUnit / ingredient.NutritionBasis.Amount;

        return new IngredientNutritionCalculationDto
        {
            IngredientId = ingredient.Id,
            IngredientName = ingredient.Name,
            UnitId = query.UnitId,
            UnitName = requestedUnitName,
            Amount = query.Amount,
            AmountInBasisUnit = amountInBasisUnit,
            NutritionBasis = new NutritionBasisDto
            {
                Amount = ingredient.NutritionBasis.Amount,
                UnitId = ingredient.NutritionBasis.UnitId,
                UnitName = basisUnitName
            },
            Nutrition = new NutritionDto
            {
                Calories = ingredient.Nutrition.Calories * scale,
                Protein = ingredient.Nutrition.Protein * scale,
                Carbohydrates = ingredient.Nutrition.Carbohydrates * scale,
                Fat = ingredient.Nutrition.Fat * scale,
                Fiber = ingredient.Nutrition.Fiber * scale
            }
        };
    }
}
