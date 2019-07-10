using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Prestige.Kernel.Authentication.Implementations;
using Prestige.Kernel.Authentication.Interfaces;
using Prestige.Kernel.Common.Constants;
using Prestige.Kernel.Common.Extensions;
using Prestige.Kernel.Common.Models.Authentication;

namespace Prestige.Kernel.Authentication.Extensions
{
    public static class JwtExtentensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            JwtOptions options = configuration.GetOptions<JwtOptions>(GlobalConstants.JwtSection);
            services.AddSingleton(options);
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = JwtSecurityKey.Create(options.SecretKey),
                        ValidIssuer = options.Issuer,
                        ValidAudience = options.ValidAudience,
                        ValidateAudience = options.ValidateAudience,
                        ValidateLifetime = options.ValidateLifetime
                    };
                });
        }
    }
}
