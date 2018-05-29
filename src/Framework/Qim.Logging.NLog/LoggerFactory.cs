using System;
using NLog;
using NLog.Config;

namespace Qim.Logging.NLog
{
    internal class LoggerFactory : ILoggerFactory
    {

        public ILogger Create(string name)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            return GetLogger(name);
        }

        public ILogger Create(Type type)
        {
            Ensure.NotNull(type, nameof(type));
            return GetLogger(type.FullName);
        }


        public static void SetXmlConfigFile(string fileName)
        {
            Ensure.NotNullOrEmpty(fileName, nameof(fileName));
            LogManager.Configuration = new XmlLoggingConfiguration(fileName);
        }

        #region private

        private ILogger GetLogger(string name)
        {
            return new QimLogger(LogManager.GetLogger(name));
        }

        #endregion
    }
}