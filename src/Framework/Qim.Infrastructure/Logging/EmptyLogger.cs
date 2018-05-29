using System;

namespace Qim.Logging
{
    /// <summary>
    /// An empty logger which log nothing.
    /// </summary>
    public class EmptyLogger : ILogger
    {
        #region ILogger Members


        public void Debug(string formatMsg, params object[] args)
        {
            
        }

        public void Debug(string message, Exception exception)
        {
            
        }

       

        public void Info(string formatMsg, params object[] args)
        {
            
        }

        public void Info(string message, Exception exception)
        {
            
        }

       
        public void Warn(string formatMsg, params object[] args)
        {
            
        }

        public void Warn(string message, Exception exception)
        {
            
        }

       

        public void Error(string formatMsg, params object[] args)
        {
            
        }

        public void Error(string message, Exception exception)
        {
            
        }

     

        public void Fatal(string formatMsg, params object[] args)
        {
            
        }

        public void Fatal(string message, Exception exception)
        {

        }
       
        #endregion

    }
}
