using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace TvMazeScraper.Common.Exception
{
    public class GlobalExceptionHandlerOptions
    {
        public static void Configure(IApplicationBuilder options)
        {
            options.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/html";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        var err = $"Error: {ex.Error.Message}";
                        await context.Response.WriteAsync(err);
                    }
                });
        }
    }
}