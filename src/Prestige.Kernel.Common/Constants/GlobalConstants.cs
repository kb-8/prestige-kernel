namespace Prestige.Kernel.Common.Constants
{
    public class GlobalConstants
    {
        // AuthenticationConstants
        public const string JwtSection = "JwtOptions";

        // ExceptionHandlerConstants
        public const string ResponseContentType = "text/html";

        public const string PublicErrorMessageFormat = "<h1>Attention! An error occurred!</h1><p> Error: {0}</p><br/>";

        // RedisConstants
        public const string RedisOptionsConnectionName = "Redis";

        // LoggingConstants
        public const string LoggingApplicationNameProperty = "ApplicationName";
        public const string ElkOptionsSectionName = "ELK";
        public const string LogstashIndexFormat = "logstash-{0:yyyy.MM.dd}";
    }
}
