using Forge.Diet.Application.DTOs;
using Forge.Diet.Application.Meals.Commands.AddMealItemToMeal;
using Forge.Diet.Application.Meals.Commands.RemoveMealItemFromMeal;
using Forge.Diet.Application.Meals.Commands.SetDailyMealSkipped;
using Forge.Diet.Application.Meals.Commands.UpdateDailyMealItemIngredients;
using Forge.Diet.Application.Meals.Commands.UpdateDailyMealItemPackedMode;
using Forge.Diet.Application.Meals.Commands.UpdateMealMealItemServings;
using Forge.Diet.Application.Meals.Queries.GetDailyMealSummary;
using Forge.Diet.Application.Meals.Queries.GetMealsByDate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Forge.Diet.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DailyMealsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DailyMealDto>))]
    public async Task<ActionResult<List<DailyMealDto>>> GetByDate(
        [FromQuery] DateOnly date,
        [FromServices] GetMealsByDateQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetMealsByDateQuery(date), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DailyMealSummaryDto))]
    public async Task<ActionResult<DailyMealSummaryDto>> GetDailySummary(
        [FromQuery] DateOnly date,
        [FromServices] GetDailyMealSummaryQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetDailyMealSummaryQuery(date), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{dailyMealId:guid}/skip")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetSkipped(
        Guid dailyMealId,
        [FromBody] SetDailyMealSkippedRequest request,
        [FromServices] SetDailyMealSkippedCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            await handler.HandleAsync(new SetDailyMealSkippedCommand(dailyMealId, request.IsSkipped), cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{dailyMealId:guid}/items")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> AddMealItem(
        Guid dailyMealId,
        [FromBody] AddMealMealItemRequest request,
        [FromServices] AddMealItemToMealCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var id = await handler.HandleAsync(
                new AddMealItemToMealCommand(
                    dailyMealId,
                    request.MealItemId,
                    request.Servings,
                    request.IsPacked,
                    request.TotalCookedWeight,
                    request.PackedWeight),
                cancellationToken);

            return CreatedAtAction(nameof(GetByDate), new { id = dailyMealId }, id);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{dailyMealId:guid}/items/{mealMealItemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMealItemServings(
        Guid dailyMealId,
        Guid mealMealItemId,
        [FromBody] UpdateMealMealItemServingsRequest request,
        [FromServices] UpdateMealMealItemServingsCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            await handler.HandleAsync(
                new UpdateMealMealItemServingsCommand(dailyMealId, mealMealItemId, request.Servings),
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{dailyMealId:guid}/items/{mealMealItemId:guid}/packed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMealItemPackedMode(
        Guid dailyMealId,
        Guid mealMealItemId,
        [FromBody] UpdateMealItemPackedModeRequest request,
        [FromServices] UpdateDailyMealItemPackedModeCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            await handler.HandleAsync(
                new UpdateDailyMealItemPackedModeCommand(
                    dailyMealId,
                    mealMealItemId,
                    request.IsPacked,
                    request.TotalCookedWeight,
                    request.PackedWeight),
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{dailyMealId:guid}/items/{mealMealItemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMealItem(
        Guid dailyMealId,
        Guid mealMealItemId,
        [FromServices] RemoveMealItemFromMealCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            await handler.HandleAsync(new RemoveMealItemFromMealCommand(dailyMealId, mealMealItemId), cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{dailyMealId:guid}/items/{mealMealItemId:guid}/ingredients")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMealItemIngredients(
        Guid dailyMealId,
        Guid mealMealItemId,
        [FromBody] UpdateDailyMealItemIngredientsRequest request,
        [FromServices] UpdateDailyMealItemIngredientsCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var commandIngredients = request.Ingredients.Select(i => 
                new UpdateDailyMealItemIngredientItemDto(i.Id, i.Quantity, i.UnitId)
            ).ToList();

            await handler.HandleAsync(
                new UpdateDailyMealItemIngredientsCommand(dailyMealId, mealMealItemId, commandIngredients),
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class UpdateDailyMealItemIngredientsRequest
{
    public List<UpdateDailyMealItemIngredientItem> Ingredients { get; set; } = new();
}

public class UpdateDailyMealItemIngredientItem
{
    public Guid Id { get; set; }
    public decimal Quantity { get; set; }
    public Guid UnitId { get; set; }
}

public class AddMealMealItemRequest
{
    public Guid MealItemId { get; set; }
    public decimal Servings { get; set; } = 1;
    public bool IsPacked { get; set; } = false;
    public decimal? TotalCookedWeight { get; set; }
    public decimal? PackedWeight { get; set; }
}

public class UpdateMealMealItemServingsRequest
{
    public decimal Servings { get; set; }
}

public class SetDailyMealSkippedRequest
{
    public bool IsSkipped { get; set; }
}

public class UpdateMealItemPackedModeRequest
{
    public bool IsPacked { get; set; }
    public decimal? TotalCookedWeight { get; set; }
    public decimal? PackedWeight { get; set; }
}
