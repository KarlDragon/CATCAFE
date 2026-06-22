namespace BE.Middleware;
using System.Text.Json;
using System.Net;
using System.Net.Mime;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate requestDelegate,
                                ILogger logger)
    {
        _next = requestDelegate;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Throw request to lower layer
            await _next(httpContext);
        }
        catch ( Exception ex)
        {
            _logger.LogError(ex, " There an error: ");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync( HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        
        var statusCode = HttpStatusCode.InternalServerError;
        var message = " Server error ";

        switch (exception)
        {
            case InvalidOperationException:
                statusCode = HttpStatusCode.Conflict;   
                message = exception.Message;
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = " Invalid credentials ";
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        var response = new { message = message };
        var jsonResponse = JsonSerializer.Serialize(response);

        return httpContext.Response.WriteAsync(jsonResponse);
    }
}