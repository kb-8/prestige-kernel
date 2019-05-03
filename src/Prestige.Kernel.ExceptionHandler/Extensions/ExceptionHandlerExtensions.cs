using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Prestige.Kernel.Common.Constants;

using System.Net;

namespace Prestige.Kernel.ExceptionHandler.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void AddExceptionHandlerSettings(this IApplicationBuilder app, bool isDevelopment)
        {
            app.UseExceptionHandler(
              options =>
              {
                  options.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.ContentType = ExceptionHandlerConstants.ResponseContentType;
                      IExceptionHandlerFeature ex = context.Features.Get<IExceptionHandlerFeature>();
                      if (ex != null)
                      {
                          string err = string.Format(ExceptionHandlerConstants.PublicErrorMessageFormat, ex.Error.Message);

                          if (isDevelopment)
                          {
                              err += ex.Error.StackTrace;
                          }

                          await context.Response.WriteAsync(err).ConfigureAwait(false);
                      }
                  });
              });
        }
    }
}
