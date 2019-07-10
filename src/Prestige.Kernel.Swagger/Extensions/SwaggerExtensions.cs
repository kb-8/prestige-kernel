using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prestige.Kernel.Common.Constants;
using Prestige.Kernel.Common.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace Prestige.Kernel.Swagger.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            SwaggerOptions options;

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
                options = configuration.GetOptions<SwaggerOptions>(GlobalConstants.SwaggerSection);
            }

            return !options.Enabled
                ? services
                : services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(options.Name, new Info { Title = options.Title, Version = options.Version });
                    if (options.IncludeSecurity)
                    {
                        c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                        {
                            Description =
                                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                            Name = "Authorization",
                            In = "header",
                            Type = "apiKey"
                        });
                    }
                });
        }

        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
        {
            SwaggerOptions options = builder.ApplicationServices.GetService<IConfiguration>()
                .GetOptions<SwaggerOptions>(GlobalConstants.SwaggerSection);

            if (!options.Enabled)
            {
                return builder;
            }

            string routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? GlobalConstants.SwaggerSection : options.RoutePrefix;

            builder.UseStaticFiles()
                .UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

            return options.ReDocEnabled
                ? builder.UseReDoc(c =>
                {
                    c.RoutePrefix = routePrefix;
                    c.SpecUrl = $"{options.Name}/swagger.json";
                })
                : builder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
                    c.RoutePrefix = routePrefix;
                });
        }
    }
}
