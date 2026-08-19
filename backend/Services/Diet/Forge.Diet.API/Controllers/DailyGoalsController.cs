using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Forge.Diet.Application.DailyGoals.Commands.UpsertDailyGoal;
using Forge.Diet.Application.DailyGoals.Queries.GetDailyGoalRange;
using Forge.Diet.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Diet.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DailyGoalsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Upsert(
        [FromBody] UpsertDailyGoalRequest request,
        [FromServices] UpsertDailyGoalCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var id = await handler.HandleAsync(
                new UpsertDailyGoalCommand(
                    request.Date,
                    request.Calories,
                    request.Protein,
                    request.Carbohydrates,
                    request.Fat,
                    request.Fiber),
                cancellationToken);

            return Ok(id);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DailySummaryRangeItemDto>))]
    public async Task<ActionResult<List<DailySummaryRangeItemDto>>> GetRangeSummary(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromServices] GetDailyGoalRangeQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetDailyGoalRangeQuery(startDate, endDate), cancellationToken);
        return Ok(result);
    }
}

public class UpsertDailyGoalRequest
{
    public DateOnly Date { get; set; }
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Fat { get; set; }
    public decimal Fiber { get; set; }
}
