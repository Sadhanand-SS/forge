using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Forge.Diet.API.Filters;

public class CancellationExceptionFilter : IExceptionFilter
{
    private readonly ILogger<CancellationExceptionFilter> _logger;

    public CancellationExceptionFilter(ILogger<CancellationExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException or TaskCanceledException)
        {
            _logger.LogInformation("Request was cancelled by the client (browser reload or tab switch).");
            context.Result = new StatusCodeResult(499); // Client Closed Request
            context.ExceptionHandled = true;
        }
    }
}
