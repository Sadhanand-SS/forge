using FluentValidation;

namespace Forge.Diet.Application.Ingredients.Queries.CalculateIngredientNutrition;

public class CalculateIngredientNutritionQueryValidator : AbstractValidator<CalculateIngredientNutritionQuery>
{
    public CalculateIngredientNutritionQueryValidator()
    {
        RuleFor(x => x.IngredientId)
            .NotEmpty()
            .WithMessage("Ingredient ID is required.");

        RuleFor(x => x.UnitId)
            .NotEmpty()
            .WithMessage("Unit ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");
    }
}
