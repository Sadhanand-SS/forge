using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forge.Diet.Application.DTOs;
using Forge.Diet.Application.Ingredients.Commands.CreateIngredient;
using Forge.Diet.Application.Ingredients.Commands.UpdateIngredient;
using Forge.Diet.Application.Ingredients.Commands.DeleteIngredient;
using Forge.Diet.Application.Ingredients.Queries.GetIngredient;
using Forge.Diet.Application.Ingredients.Queries.SearchIngredients;
using Forge.Diet.Application.Ingredients.Queries.CalculateIngredientNutrition;

namespace Forge.Diet.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class IngredientsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedListDto<IngredientDto>))]
    public async Task<ActionResult<PaginatedListDto<IngredientDto>>> Get(
        [FromQuery] string? search,
        [FromQuery] string? brand,
        [FromQuery] string? cursor,
        [FromServices] SearchIngredientsQueryHandler handler,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await handler.HandleAsync(new SearchIngredientsQuery(search, brand, cursor, limit), cancellationToken);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499); // Client Closed Request
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IngredientDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IngredientDto>> GetById(
        Guid id,
        [FromServices] GetIngredientQueryHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.HandleAsync(new GetIngredientQuery(id), cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499);
        }
    }

    [HttpGet("{ingredientId:guid}/nutrition")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IngredientNutritionCalculationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IngredientNutritionCalculationDto>> GetNutrition(
        Guid ingredientId,
        [FromQuery] Guid unitId,
        [FromQuery] decimal amount,
        [FromServices] IValidator<CalculateIngredientNutritionQuery> validator,
        [FromServices] CalculateIngredientNutritionQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new CalculateIngredientNutritionQuery(ingredientId, unitId, amount);
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await handler.HandleAsync(query, cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateIngredientCommand command,
        [FromServices] CreateIngredientCommandHandler handler,
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
        catch (OperationCanceledException)
        {
            return StatusCode(499);
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateIngredientCommand command,
        [FromServices] UpdateIngredientCommandHandler handler,
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
        catch (OperationCanceledException)
        {
            return StatusCode(499);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteIngredientCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            await handler.HandleAsync(new DeleteIngredientCommand(id), cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499);
        }
    }
}
