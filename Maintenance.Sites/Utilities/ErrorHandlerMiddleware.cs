using Fanex.Logging.Extensions.Database;
using Fanex.Logging.Extensions;

namespace Maintenance.Sites.Utilities;

public class ErrorHandlerMiddleware(RequestDelegate next, Fanex.Logging.ILogger logger, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        var logMessage = new LogMessage(exception.Message)
            .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
            .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
            .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
            .ToString();
        await logger.ErrorAsync(logMessage);

        var requestedWith = context.Request.Headers["X-Requested-With"];

        var isAjaxRequest = requestedWith == "XMLHttpRequest";
        if (!context.Response.HasStarted && isAjaxRequest)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        await context.Response.WriteAsync(exception.Message);

        var env = configuration.GetSection("Fanex.NAP.Client:Environment").Value;

        if (!env.Equals("LIVE", StringComparison.CurrentCultureIgnoreCase))
        {
            await context.Response.WriteAsync(Environment.NewLine);
            await context.Response.WriteAsync(exception.StackTrace);
        }
    }
}