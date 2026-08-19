using System;

namespace Forge.Diet.Application.DTOs;

public class DailySummaryRangeItemDto
{
    public DateOnly Date { get; set; }
    public decimal EatenCalories { get; set; }
    public decimal EatenProtein { get; set; }
    public decimal EatenCarbohydrates { get; set; }
    public decimal EatenFat { get; set; }
    public decimal EatenFiber { get; set; }
    public DailyGoalDto Goal { get; set; } = null!;
}
