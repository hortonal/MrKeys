using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Logging
{
    public class NullLogger : ILogger
    {
        public LogLevel LogLevel
        {
            set
            {
                
            }
        }

        public void Log(LogLevel level, string message)
        {
            
        }

        public void Log(object raiser, LogLevel level, string message)
        {
            
        }
    }
}
