using System;
using System.Collections.Generic;

namespace Forge.Diet.Domain.Entities;

public class Meal
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public TimeOnly Time { get; private set; }

    public bool IsSystem { get; private set; }

    public ICollection<DailyMeal> DailyMeals { get; private set; }

    private Meal()
    {
        Name = null!;
        DailyMeals = new List<DailyMeal>();
    }

    public Meal(Guid id, string name, TimeOnly time, bool isSystem)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Meal name cannot be empty.", nameof(name));
        }

        Id = id;
        Name = name.Trim();
        Time = time;
        IsSystem = isSystem;
        DailyMeals = new List<DailyMeal>();
    }

    public void Update(string name, TimeOnly time)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Meal name cannot be empty.", nameof(name));
        }

        name = name.Trim();

        if (IsSystem && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("System meals cannot be renamed.");
        }

        Name = name;
        Time = time;
    }
}
