using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Logging
{
    public class DebugOutputLogger : ILogger
    {
        private LogLevel _logLevel;

        public LogLevel LogLevel
        {
            set
            {
                _logLevel = value;
            }
        }

        public void Log(LogLevel level, string message)
        {
            //Perhaps convert to string builder. don't care too much..
            Debug.WriteLine(BuildMessage(level, message));
        }

        public void Log(object raiser, LogLevel level, string message)
        {
            Debug.WriteLine(raiser, BuildMessage(level, message));
        }

        private string BuildMessage(LogLevel level, string message)
        {
            return DateTime.UtcNow + "" + DateTime.UtcNow.Millisecond + ", " + LogLevelAsString(level) + ", " + message;
        }

        private string LogLevelAsString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Critical:
                    return "Critical";
                case LogLevel.Debug:
                    return "Debug";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Info:
                    return "Info";
                case LogLevel.Warning:
                    return "Warning";
                default:
                    return "Unknown log level";
            }
        }

    }
}
