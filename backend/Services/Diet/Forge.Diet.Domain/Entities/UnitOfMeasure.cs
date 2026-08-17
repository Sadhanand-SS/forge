using System;

namespace Forge.Diet.Domain.Entities;

public class UnitOfMeasure
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public bool IsSystem { get; private set; }

    private UnitOfMeasure()
    {
        Name = null!;
        Description = null!;
    }

    public UnitOfMeasure(Guid id, string name, string description, bool isSystem)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Unit name cannot be empty.", nameof(name));
        }

        Id = id;
        Name = name;
        Description = description ?? "";
        IsSystem = isSystem;
    }

    public void Update(string name, string description)
    {
        if (IsSystem)
        {
            throw new InvalidOperationException("Cannot update system-defined units of measure.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Unit name cannot be empty.", nameof(name));
        }

        Name = name;
        Description = description ?? "";
    }
}
