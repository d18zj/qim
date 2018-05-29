using System;

namespace Qim.Ioc.Tests.Interception
{
    public class Tracer
    {
        private string _traceLog = string.Empty;
        public void Trace(Type type)
        {
            _traceLog = Trace(type, _traceLog);
        }

        public bool ValidTraceLog(Type[] types)
        {
            string result = string.Empty;
            foreach (var type in types)
            {
                result = Trace(type, result);
            }
            return result == _traceLog;
        }

        public string GetTraceLog()
        {
            return _traceLog;
        }

        public void ResetTrace()
        {
            _traceLog = string.Empty;
        }

        private string Trace(Type type, string log)
        {
            if (string.IsNullOrEmpty(log))
            {
                log = type.FullName;
            }
            else
            {
                log = log + "-" + type.FullName;
            }
            return log;
        }
    }
}