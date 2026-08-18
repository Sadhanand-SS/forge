using System;

namespace Forge.Diet.Domain.Constants;

public static class DefaultMeals
{
    public static readonly Guid BreakfastId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid LunchId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid DinnerId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public const string Breakfast = "Breakfast";
    public const string Lunch = "Lunch";
    public const string Dinner = "Dinner";

    public static readonly TimeOnly BreakfastTime = new(8, 0);
    public static readonly TimeOnly LunchTime = new(13, 0);
    public static readonly TimeOnly DinnerTime = new(20, 0);
}
