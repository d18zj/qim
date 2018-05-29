using System.Collections.Generic;
using Qim.Util;

namespace Qim.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration()
        {
            ConfigDictionary = new Dictionary<string, object>();
        }

        internal IDictionary<string, object> ConfigDictionary { get; set; }

        public string DefaultNameOrConnectionString
        {
            get { return Get("DefaultNameOrConnectionString", ""); }
            set { Set("DefaultNameOrConnectionString", value); }
        }

        public string BaseAppPath
        {
            get
            {
                var path = Get<string>("BaseAppPath", null);
                if (string.IsNullOrEmpty(path))
                {
                    path = UtilHelper.GetCurrentBinDirectory();
                    BaseAppPath = path;
                }
                return path;
            }
            set { Set("BaseAppPath", value); }
        }

        protected T Get<T>(string name, T defaultValue)
        {
            object result;
            if (ConfigDictionary.TryGetValue(name, out result))
            {
                return (T) result;
            }
            return defaultValue;
        }

        protected void Set(string name, object value)
        {
            ConfigDictionary[name] = value;
        }
    }
}