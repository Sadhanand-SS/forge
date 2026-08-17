using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;

namespace Forge.Diet.Application.Ingredients.Queries.SearchIngredients;

public record SearchIngredientsQuery(string? Name, string? Brand);

public class SearchIngredientsQueryHandler
{
    private readonly IDietDbContext _context;

    public SearchIngredientsQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<List<IngredientDto>> HandleAsync(SearchIngredientsQuery query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.Ingredients
            .Include(i => i.Conversions)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            dbQuery = dbQuery.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.Brand))
        {
            dbQuery = dbQuery.Where(i => i.Brand != null && i.Brand.ToLower().Contains(query.Brand.ToLower()));
        }

        var list = await dbQuery.OrderBy(i => i.Name).ToListAsync(cancellationToken);

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, cancellationToken);

        string getUnitName(Guid id) => units.TryGetValue(id, out var name) ? name : "Unknown";

        return list.Select(ingredient => new IngredientDto
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
        }).ToList();
    }
}
