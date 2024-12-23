using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Net;
using System.Text.Json;

namespace BackgroundTask.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring global exception handling in a web application.
    /// </summary>
    /// <remarks>
    /// The <see cref="ConfigureExceptionHandlerExtension"/> class defines a static method that sets up exception handling 
    /// for the entire application. This ensures that any unhandled exceptions are caught, logged, and a standardized error 
    /// response is returned to the client.
    /// </remarks>
    
    public static class ConfigureExceptionHandlerExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication application)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async (context) =>
                {
                    context.Response.StatusCode = (int)(HttpStatusCode.InternalServerError);
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature is not null)
                    {
                        Console.WriteLine(contextFeature.Error.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                            Message = contextFeature.Error.Message,
                            Title = "Unexpected Error"
                        }));
                    };
                });
            });
        }
    }
}