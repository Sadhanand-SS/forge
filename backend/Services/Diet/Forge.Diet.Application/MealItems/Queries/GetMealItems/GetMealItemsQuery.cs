using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;

namespace Forge.Diet.Application.MealItems.Queries.GetMealItems;

public record GetMealItemsQuery();

public class GetMealItemsQueryHandler
{
    private readonly IDietDbContext _context;

    public GetMealItemsQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<List<MealItemDto>> HandleAsync(GetMealItemsQuery query, CancellationToken cancellationToken = default)
    {
        var mealItems = await _context.MealItems
            .Include(m => m.Ingredients)
                .ThenInclude(i => i.Ingredient)
                    .ThenInclude(ing => ing.Conversions)
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .ToListAsync(CancellationToken.None);

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, CancellationToken.None);

        string getUnitName(Guid id) => units.TryGetValue(id, out var name) ? name : "Unknown";

        return mealItems.Select(mealItem =>
        {
            var totalNutrition = mealItem.GetTotalNutrition();

            return new MealItemDto
            {
                Id = mealItem.Id,
                Name = mealItem.Name,
                Description = mealItem.Description,
                Ingredients = mealItem.Ingredients.Select(item => new MealIngredientDto
                {
                    Id = item.Id,
                    IngredientId = item.IngredientId,
                    IngredientName = item.Ingredient.Name,
                    Quantity = item.Quantity,
                    UnitId = item.UnitId,
                    UnitName = getUnitName(item.UnitId)
                }).ToList(),
                TotalNutrition = new NutritionDto
                {
                    Calories = totalNutrition.Calories,
                    Protein = totalNutrition.Protein,
                    Carbohydrates = totalNutrition.Carbohydrates,
                    Fat = totalNutrition.Fat,
                    Fiber = totalNutrition.Fiber
                }
            };
        }).ToList();
    }
}
