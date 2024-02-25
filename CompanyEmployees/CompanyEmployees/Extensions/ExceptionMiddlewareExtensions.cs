using System.Net;
using Contracts;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
    {
        app.UseExceptionHandler((IApplicationBuilder appBuilder) =>
        {
            appBuilder.Run(async (HttpContext context) =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsync(GenerateError(context).ToString());
                }
            });
        });
    }

    private static ErrorDetails GenerateError(HttpContext context) =>
        new ()
        {
            StatusCode = context.Response.StatusCode,
            Message    = "Internal Server Error.",
        };
}