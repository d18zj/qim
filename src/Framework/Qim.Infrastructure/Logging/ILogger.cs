using System;

namespace Qim.Logging
{
    /// <summary>
    /// Represents a logger interface.
    /// </summary>
    public interface ILogger
    {

        /// <summary>
        /// Write a debug level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Debug(string message, params object[] args);

        /// <summary>
        /// Write a debug level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Debug(string message, Exception exception);

       

        /// <summary>
        /// Write a info level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Info(string message, params object[] args);


        /// <summary>
        /// Write a info level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Info(string message, Exception exception);

     

        /// <summary>
        /// Write a warnning level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Warn(string message, params object[] args);

        /// <summary>
        /// Write a warnning level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Warn(string message, Exception exception);


        /// <summary>
        /// Write an error level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Error(string message, params object[] args);

        /// <summary>Write an error level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Error(string message, Exception exception);

     
        /// <summary>
        /// Write a fatal level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Fatal(string message, params object[] args);

        /// <summary>
        /// Write a fatal level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Fatal(string message, Exception exception);
    }
}