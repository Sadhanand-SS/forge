using Forge.Diet.Domain.Constants;
using Forge.Diet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forge.Diet.Infrastructure.Persistence;

public static class DietDbInitializer
{
    public static async Task InitializeAsync(DietDbContext context, CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);

        var existingSystemUnitIds = await context.UnitsOfMeasure
            .Where(unit => unit.Id == SystemUnits.Gram
                || unit.Id == SystemUnits.Milliliter
                || unit.Id == SystemUnits.Piece)
            .Select(unit => unit.Id)
            .ToListAsync(cancellationToken);

        var systemUnitsToAdd = new List<UnitOfMeasure>();

        if (!existingSystemUnitIds.Contains(SystemUnits.Gram))
        {
            systemUnitsToAdd.Add(new UnitOfMeasure(
                SystemUnits.Gram,
                "Gram",
                "Standard metric gram",
                isSystem: true));
        }

        if (!existingSystemUnitIds.Contains(SystemUnits.Milliliter))
        {
            systemUnitsToAdd.Add(new UnitOfMeasure(
                SystemUnits.Milliliter,
                "Milliliter",
                "Standard metric milliliter",
                isSystem: true));
        }

        if (!existingSystemUnitIds.Contains(SystemUnits.Piece))
        {
            systemUnitsToAdd.Add(new UnitOfMeasure(
                SystemUnits.Piece,
                "Piece",
                "Individual item/serving count",
                isSystem: true));
        }

        if (systemUnitsToAdd.Count == 0)
        {
            await EnsureSystemMealsAsync(context, cancellationToken);
            return;
        }

        await context.UnitsOfMeasure.AddRangeAsync(systemUnitsToAdd, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await EnsureSystemMealsAsync(context, cancellationToken);
    }

    private static async Task EnsureSystemMealsAsync(DietDbContext context, CancellationToken cancellationToken)
    {
        var existingSystemMealIds = await context.Meals
            .Where(meal => meal.Id == DefaultMeals.BreakfastId
                || meal.Id == DefaultMeals.LunchId
                || meal.Id == DefaultMeals.DinnerId)
            .Select(meal => meal.Id)
            .ToListAsync(cancellationToken);

        var mealsToAdd = new List<Meal>();

        if (!existingSystemMealIds.Contains(DefaultMeals.BreakfastId))
        {
            mealsToAdd.Add(new Meal(DefaultMeals.BreakfastId, DefaultMeals.Breakfast, DefaultMeals.BreakfastTime, isSystem: true));
        }

        if (!existingSystemMealIds.Contains(DefaultMeals.LunchId))
        {
            mealsToAdd.Add(new Meal(DefaultMeals.LunchId, DefaultMeals.Lunch, DefaultMeals.LunchTime, isSystem: true));
        }

        if (!existingSystemMealIds.Contains(DefaultMeals.DinnerId))
        {
            mealsToAdd.Add(new Meal(DefaultMeals.DinnerId, DefaultMeals.Dinner, DefaultMeals.DinnerTime, isSystem: true));
        }

        if (mealsToAdd.Count == 0)
        {
            return;
        }

        await context.Meals.AddRangeAsync(mealsToAdd, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
