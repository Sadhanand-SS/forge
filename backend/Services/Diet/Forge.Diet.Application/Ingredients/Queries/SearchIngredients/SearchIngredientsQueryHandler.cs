using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;

namespace Forge.Diet.Application.Ingredients.Queries.SearchIngredients;

public class SearchIngredientsQueryHandler
{
    private readonly IDietDbContext _context;

    public SearchIngredientsQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedListDto<IngredientDto>> HandleAsync(SearchIngredientsQuery query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.Ingredients
            .Include(i => i.Conversions)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            var searchTerm = query.Name.ToLower();
            dbQuery = dbQuery.Where(i => 
                i.Name.ToLower().Contains(searchTerm) || 
                (i.Brand != null && i.Brand.ToLower().Contains(searchTerm)) ||
                (i.Brand != null && (i.Brand.ToLower() + " " + i.Name.ToLower()).Contains(searchTerm)) ||
                (i.Name.ToLower() + " " + (i.Brand != null ? i.Brand.ToLower() : "")).Contains(searchTerm)
            );
        }

        if (!string.IsNullOrWhiteSpace(query.Brand))
        {
            dbQuery = dbQuery.Where(i => i.Brand != null && i.Brand.ToLower().Contains(query.Brand.ToLower()));
        }

        // Apply cursor pagination
        if (!string.IsNullOrEmpty(query.Cursor))
        {
            try
            {
                var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(query.Cursor));
                var parts = decoded.Split('|');
                if (parts.Length == 2)
                {
                    var cursorName = parts[0];
                    var cursorId = Guid.Parse(parts[1]);

                    dbQuery = dbQuery.Where(i => 
                        string.Compare(i.Name, cursorName) > 0 || 
                        (i.Name == cursorName && i.Id.CompareTo(cursorId) > 0));
                }
            }
            catch
            {
                // Ignore invalid cursor
            }
        }

        // Sort by Name, then by Id
        dbQuery = dbQuery.OrderBy(i => i.Name).ThenBy(i => i.Id);

        // Fetch limit + 1 to see if there is a next page
        var fetchLimit = query.Limit > 0 ? query.Limit : 20;
        var list = await dbQuery.Take(fetchLimit + 1).ToListAsync(CancellationToken.None);

        var hasNextPage = list.Count > fetchLimit;
        if (hasNextPage)
        {
            list.RemoveAt(list.Count - 1);
        }

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, CancellationToken.None);

        string getUnitName(Guid id) => units.TryGetValue(id, out var name) ? name : "Unknown";

        var items = list.Select(ingredient => new IngredientDto
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

        string? nextCursor = null;
        if (hasNextPage && items.Count > 0)
        {
            var lastItem = items.Last();
            nextCursor = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{lastItem.Name}|{lastItem.Id}"));
        }

        return new PaginatedListDto<IngredientDto>
        {
            Items = items,
            NextCursor = nextCursor
        };
    }
}
