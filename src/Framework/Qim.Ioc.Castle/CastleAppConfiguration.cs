using Qim.Configuration;

namespace Qim.Ioc.Castle
{
    internal class CastleAppConfiguration : IocAppConfiguration
    {
        public CastleAppConfiguration(IAppConfiguration configuration)
        {
            ConfigDictionary = configuration.ConfigDictionary;
        }

      
    }
}