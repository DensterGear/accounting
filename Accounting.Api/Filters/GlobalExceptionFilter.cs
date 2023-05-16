using System.Net;
using Accounting.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Accounting.Api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var response = new ErrorResponse(exception.Message);
        context.Result = new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
        
        _logger.LogError(exception, "An error occurred during the execution of the request.");
    }
}
