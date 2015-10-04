using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Logging
{
    public interface ILogger
    {
        LogLevel LogLevel { set; }
        void Log(LogLevel level, string message);
        void Log(object raiser, LogLevel level, string message);
    }

    public enum LogLevel { Debug, Info, Warning, Error, Critical, OMG }
}
