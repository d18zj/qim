using Qim.Configuration;
using Qim.Ioc;

namespace Qim.Logging.NLog
{
    public static class ConfigurationExtensions
    {
        public static IIocAppConfiguration UseNLog(this IIocAppConfiguration configuration, string configFile = null)
        {
            if (!string.IsNullOrEmpty(configFile))
            {
                LoggerFactory.SetXmlConfigFile(configFile);
            }

            configuration.Registrar.Register<ILoggerFactory, LoggerFactory>(LifetimeType.Singleton);
            return configuration;
        }
    }
}