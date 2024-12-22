using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Net;
using System.Text.Json;

namespace BackgroundTask.API.Extensions
{
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