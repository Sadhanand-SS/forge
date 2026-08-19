using System;

namespace Forge.Diet.Domain.Entities;

public class DailyGoal
{
    public Guid Id { get; private set; }

    public DateOnly Date { get; private set; }

    public decimal Calories { get; private set; }

    public decimal Protein { get; private set; }

    public decimal Carbohydrates { get; private set; }

    public decimal Fat { get; private set; }

    public decimal Fiber { get; private set; }

    private DailyGoal()
    {
    }

    public DailyGoal(
        Guid id,
        DateOnly date,
        decimal calories,
        decimal protein,
        decimal carbohydrates,
        decimal fat,
        decimal fiber)
    {
        if (calories < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(calories), "Calories cannot be negative.");
        }

        if (protein < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(protein), "Protein cannot be negative.");
        }

        if (carbohydrates < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(carbohydrates), "Carbohydrates cannot be negative.");
        }

        if (fat < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fat), "Fat cannot be negative.");
        }

        if (fiber < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fiber), "Fiber cannot be negative.");
        }

        Id = id;
        Date = date;
        Calories = calories;
        Protein = protein;
        Carbohydrates = carbohydrates;
        Fat = fat;
        Fiber = fiber;
    }

    public void Update(
        decimal calories,
        decimal protein,
        decimal carbohydrates,
        decimal fat,
        decimal fiber)
    {
        if (calories < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(calories), "Calories cannot be negative.");
        }

        if (protein < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(protein), "Protein cannot be negative.");
        }

        if (carbohydrates < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(carbohydrates), "Carbohydrates cannot be negative.");
        }

        if (fat < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fat), "Fat cannot be negative.");
        }

        if (fiber < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fiber), "Fiber cannot be negative.");
        }

        Calories = calories;
        Protein = protein;
        Carbohydrates = carbohydrates;
        Fat = fat;
        Fiber = fiber;
    }
}
