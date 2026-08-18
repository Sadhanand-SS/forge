using System;
namespace Forge.Diet.Application.DTOs;

public class MealDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public TimeOnly Time { get; set; }
    public bool IsSystem { get; set; }
}
