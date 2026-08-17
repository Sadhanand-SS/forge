using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Application.DTOs;

namespace Forge.Diet.Application.MealItems.Queries.GetMealItem;

public record GetMealItemQuery(Guid Id);

public class GetMealItemQueryHandler
{
    private readonly IDietDbContext _context;

    public GetMealItemQueryHandler(IDietDbContext context)
    {
        _context = context;
    }

    public async Task<MealItemDto> HandleAsync(GetMealItemQuery query, CancellationToken cancellationToken = default)
    {
        var mealItem = await _context.MealItems
            .Include(m => m.Ingredients)
                .ThenInclude(i => i.Ingredient)
                    .ThenInclude(ing => ing.Conversions)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == query.Id, cancellationToken);

        if (mealItem == null)
        {
            throw new KeyNotFoundException($"Meal item with ID '{query.Id}' was not found.");
        }

        var totalNutrition = mealItem.GetTotalNutrition();

        var units = await _context.UnitsOfMeasure
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Name, cancellationToken);

        string getUnitName(Guid id) => units.TryGetValue(id, out var name) ? name : "Unknown";

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
    }
}
