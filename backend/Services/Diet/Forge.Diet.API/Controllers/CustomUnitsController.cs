using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forge.Diet.Application.DTOs;
using Forge.Diet.Application.CustomUnits.Commands.CreateCustomUnit;
using Forge.Diet.Application.CustomUnits.Commands.UpdateCustomUnit;
using Forge.Diet.Application.CustomUnits.Commands.DeleteCustomUnit;
using Forge.Diet.Application.CustomUnits.Queries.GetCustomUnits;

namespace Forge.Diet.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomUnitsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UnitOfMeasureDto>))]
    public async Task<ActionResult<List<UnitOfMeasureDto>>> Get(
        [FromServices] GetCustomUnitsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetCustomUnitsQuery();
            var result = await handler.HandleAsync(query, cancellationToken);
            return Ok(result);
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
        [FromBody] CreateCustomUnitRequest request,
        [FromServices] CreateCustomUnitCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateCustomUnitCommand(request.Name, request.Description);
            var result = await handler.HandleAsync(command, cancellationToken);
            return CreatedAtAction(nameof(Get), result);
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

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCustomUnitRequest request,
        [FromServices] UpdateCustomUnitCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateCustomUnitCommand(id, request.Name, request.Description);
            await handler.HandleAsync(command, cancellationToken);
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
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteCustomUnitCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteCustomUnitCommand(id);
            await handler.HandleAsync(command, cancellationToken);
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

public class CreateCustomUnitRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class UpdateCustomUnitRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
