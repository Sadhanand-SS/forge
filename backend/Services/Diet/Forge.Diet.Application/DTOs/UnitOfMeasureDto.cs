using System;

namespace Forge.Diet.Application.DTOs;

public class UnitOfMeasureDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsSystem { get; set; }
}
