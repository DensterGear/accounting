using Accounting.Api.Filters;
using Accounting.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;

namespace Accounting.Api.Tests.Filters;

[TestClass]
public class GlobalExceptionFilterTests
{
    [TestMethod]
    public void OnException_Should_Return_InternalServerError_With_ErrorResponse()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionFilter>>();
        var exceptionContext = new ExceptionContext(new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
        }, new [] { new GlobalExceptionFilter(loggerMock.Object) });
        const string expectedMessage = "Test exception message";
        exceptionContext.Exception = new Exception(expectedMessage);

        // Act
        new GlobalExceptionFilter(loggerMock.Object).OnException(exceptionContext);

        // Assert
        var result = exceptionContext.Result as ObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        
        var errorResponse = result.Value as ErrorResponse;
        Assert.IsNotNull(errorResponse);
        Assert.AreEqual(expectedMessage, errorResponse.Message);
    }
}