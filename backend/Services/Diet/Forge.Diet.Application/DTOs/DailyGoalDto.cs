using System;

namespace Forge.Diet.Application.DTOs;

public class DailyGoalDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Fat { get; set; }
    public decimal Fiber { get; set; }
}
