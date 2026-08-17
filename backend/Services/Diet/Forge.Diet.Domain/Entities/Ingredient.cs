using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Diet.Domain.ValueObjects;

namespace Forge.Diet.Domain.Entities;

public class Ingredient
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string? Brand { get; private set; }

    public decimal NetWeight { get; private set; }

    public NutritionBasis NutritionBasis { get; private set; }

    public Nutrition Nutrition { get; private set; }

    public ICollection<IngredientConversion> Conversions { get; private set; }

    private Ingredient()
    {
        Name = null!;
        NutritionBasis = null!;
        Nutrition = null!;
        Conversions = new List<IngredientConversion>();
    }

    public Ingredient(Guid id, string name, string? brand, decimal netWeight, NutritionBasis nutritionBasis, Nutrition nutrition)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Ingredient name cannot be empty.", nameof(name));
        }

        if (netWeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(netWeight), "Net weight must be greater than zero.");
        }

        Id = id;
        Name = name;
        Brand = brand;
        NetWeight = netWeight;
        NutritionBasis = nutritionBasis ?? throw new ArgumentNullException(nameof(nutritionBasis));
        Nutrition = nutrition ?? throw new ArgumentNullException(nameof(nutrition));
        Conversions = new List<IngredientConversion>();
    }

    public void Update(string name, string? brand, decimal netWeight, NutritionBasis nutritionBasis, Nutrition nutrition)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Ingredient name cannot be empty.", nameof(name));
        }

        if (netWeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(netWeight), "Net weight must be greater than zero.");
        }

        Name = name;
        Brand = brand;
        NetWeight = netWeight;
        NutritionBasis = nutritionBasis ?? throw new ArgumentNullException(nameof(nutritionBasis));
        Nutrition = nutrition ?? throw new ArgumentNullException(nameof(nutrition));
    }

    public void AddConversion(Guid targetUnitId, decimal conversionFactor)
    {
        if (targetUnitId == NutritionBasis.UnitId)
        {
            throw new InvalidOperationException("Cannot add a conversion rate for the default basis unit of measure.");
        }

        var existing = Conversions.FirstOrDefault(c => c.TargetUnitId == targetUnitId);
        if (existing != null)
        {
            Conversions.Remove(existing);
        }

        Conversions.Add(new IngredientConversion(Guid.NewGuid(), targetUnitId, conversionFactor));
    }

    public void RemoveConversion(Guid conversionId)
    {
        var conversion = Conversions.FirstOrDefault(c => c.Id == conversionId);
        if (conversion != null)
        {
            Conversions.Remove(conversion);
        }
    }

    public void ClearConversions()
    {
        Conversions.Clear();
    }

    public bool IsCompatibleUnit(Guid unitId)
    {
        return NutritionBasis.UnitId == unitId || Conversions.Any(c => c.TargetUnitId == unitId);
    }

    public decimal GetConversionFactor(Guid unitId)
    {
        if (unitId == NutritionBasis.UnitId)
        {
            return 1.0m;
        }

        var conversion = Conversions.FirstOrDefault(c => c.TargetUnitId == unitId);
        if (conversion == null)
        {
            throw new InvalidOperationException($"Unit with ID '{unitId}' is not compatible with Ingredient '{Name}' basis unit '{NutritionBasis.UnitId}'.");
        }

        return conversion.ConversionFactor;
    }
}
