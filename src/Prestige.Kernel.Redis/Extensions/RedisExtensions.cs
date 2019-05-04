using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Prestige.Kernel.Common.Constants;
using Prestige.Kernel.Common.Extensions;
using Prestige.Kernel.Common.Models.Redis;

namespace Prestige.Kernel.Redis.Extensions
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            IConfigurationSection redisSection = configuration.GetSection(GlobalConstants.RedisOptionsConnectionName);
            RedisOptions options = configuration.GetOptions<RedisOptions>(GlobalConstants.RedisOptionsConnectionName);
            services.AddDistributedRedisCache(o =>
            {
                o.Configuration = options.ConnectionString;
                o.InstanceName = options.Instance;
            });

            return services;
        }
    }
}
