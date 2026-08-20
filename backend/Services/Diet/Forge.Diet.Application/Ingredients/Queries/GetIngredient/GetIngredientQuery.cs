using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;

namespace Forge.Diet.Application.Ingredients.Queries.GetIngredient;

public record GetIngredientQuery(Guid Id);

public class GetIngredientQueryHandler
{
    private readonly IDietDbContext _context;

    public GetIngredientQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<IngredientDto> HandleAsync(GetIngredientQuery query, CancellationToken cancellationToken = default)
    {
        var ingredient = await _context.Ingredients
            .Include(i => i.Conversions)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == query.Id, cancellationToken);

        if (ingredient == null)
        {
            throw new KeyNotFoundException($"Ingredient with ID '{query.Id}' was not found.");
        }

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, CancellationToken.None);

        string getUnitName(Guid id) => units.TryGetValue(id, out var name) ? name : "Unknown";

        return new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Brand = ingredient.Brand,
            NetWeight = ingredient.NetWeight,
            NutritionBasis = new NutritionBasisDto
            {
                Amount = ingredient.NutritionBasis.Amount,
                UnitId = ingredient.NutritionBasis.UnitId,
                UnitName = getUnitName(ingredient.NutritionBasis.UnitId)
            },
            Nutrition = new NutritionDto
            {
                Calories = ingredient.Nutrition.Calories,
                Protein = ingredient.Nutrition.Protein,
                Carbohydrates = ingredient.Nutrition.Carbohydrates,
                Fat = ingredient.Nutrition.Fat,
                Fiber = ingredient.Nutrition.Fiber
            },
            Conversions = ingredient.Conversions.Select(c => new IngredientConversionDto
            {
                Id = c.Id,
                TargetUnitId = c.TargetUnitId,
                TargetUnitName = getUnitName(c.TargetUnitId),
                ConversionFactor = c.ConversionFactor
            }).ToList()
        };
    }
}
