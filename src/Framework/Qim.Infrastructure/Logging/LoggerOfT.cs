using System;

namespace Qim.Logging
{
    public class Logger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public Logger(ILoggerFactory factory)
        {
            Ensure.NotNull(factory, nameof(factory));
            _logger = factory.Create(typeof(T));
        }

        public void Debug(string message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        public void Fatal(string message, params object[] args)
        {
            _logger.Fatal(message, args);
        }

        public void Info(string message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void Warn(string message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        public void Warn(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }
    }
}
