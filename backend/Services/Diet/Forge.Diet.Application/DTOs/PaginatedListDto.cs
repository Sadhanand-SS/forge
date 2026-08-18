using System.Collections.Generic;

namespace Forge.Diet.Application.DTOs;

public class PaginatedListDto<T>
{
    public List<T> Items { get; set; } = new();
    public string? NextCursor { get; set; }
}
