using System.Collections.Generic;

namespace Qim.Ioc.LightInject
{
    internal class IocAppConfiguration : Qim.Configuration.IocAppConfiguration
    {
        public IocAppConfiguration(IDictionary<string, object> dicts)
        {
            ConfigDictionary = dicts;
        }
    }
}