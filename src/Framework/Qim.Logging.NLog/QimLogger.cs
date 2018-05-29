using System;
using NLog;

namespace Qim.Logging.NLog
{
    internal class QimLogger : ILogger
    {
        private readonly Logger _logger;

        public QimLogger(Logger logger)
        {
            _logger = logger;
        }



        public void Debug(string formatMsg, params object[] args)
        {
            _logger.Debug(formatMsg, args);
        }

        public void Debug(string message, Exception exception)
        {
            _logger.Debug(exception, message);
        }


        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void Info(string message, Exception exception)
        {
            _logger.Info(exception, message);
        }



        public void Warn(string formatMsg, params object[] args)
        {
            _logger.Warn(formatMsg, args);
        }

        public void Warn(string message, Exception exception)
        {
            _logger.Warn(exception, message);
        }



        public void Error(string formatMsg, params object[] args)
        {
            _logger.Error(formatMsg, args);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }



        public void Fatal(string formatMsg, params object[] args)
        {
            _logger.Fatal(formatMsg, args);
        }

        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }
    }
}