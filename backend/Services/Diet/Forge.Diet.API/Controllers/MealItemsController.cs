using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forge.Diet.Application.DTOs;
using Forge.Diet.Application.MealItems.Commands.CreateMealItem;
using Forge.Diet.Application.MealItems.Commands.UpdateMealItem;
using Forge.Diet.Application.MealItems.Commands.DeleteMealItem;
using Forge.Diet.Application.MealItems.Commands.AddIngredientToMealItem;
using Forge.Diet.Application.MealItems.Commands.RemoveIngredientFromMealItem;
using Forge.Diet.Application.MealItems.Queries.GetMealItem;
using Forge.Diet.Application.MealItems.Queries.GetMealItems;

namespace Forge.Diet.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MealItemsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MealItemDto>))]
    public async Task<ActionResult<List<MealItemDto>>> Get(
        [FromServices] GetMealItemsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetMealItemsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MealItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MealItemDto>> GetById(
        Guid id,
        [FromServices] GetMealItemQueryHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.HandleAsync(new GetMealItemQuery(id), cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateMealItemCommand command,
        [FromServices] CreateMealItemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var id = await handler.HandleAsync(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateMealItemCommand command,
        [FromServices] UpdateMealItemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match ID in body.");
        }

        try
        {
            await handler.HandleAsync(command, cancellationToken);
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteMealItemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            await handler.HandleAsync(new DeleteMealItemCommand(id), cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Meal Ingredients endpoints

    [HttpPost("{mealItemId:guid}/ingredients")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> AddIngredient(
        Guid mealItemId,
        [FromBody] AddMealIngredientRequest request,
        [FromServices] AddIngredientToMealItemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new AddIngredientToMealItemCommand(mealItemId, request.IngredientId, request.Quantity, request.UnitId);
            var ingredientId = await handler.HandleAsync(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = mealItemId }, ingredientId);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{mealItemId:guid}/ingredients/{mealIngredientId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveIngredient(
        Guid mealItemId,
        Guid mealIngredientId,
        [FromServices] RemoveIngredientFromMealItemCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new RemoveIngredientFromMealItemCommand(mealItemId, mealIngredientId);
            await handler.HandleAsync(command, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

public class AddMealIngredientRequest
{
    public Guid IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public Guid UnitId { get; set; }
}
