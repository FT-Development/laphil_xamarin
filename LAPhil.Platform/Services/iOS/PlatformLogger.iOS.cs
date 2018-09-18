﻿#if __IOS__
using System;
using Serilog;
using LAPhil.Logging;


namespace LAPhil.Platform
{
    public class PlatfromLogger : IPlatformLogger
    {
        public LoggerConfiguration Configure(LoggerConfiguration config)
        {
            return config.WriteTo.NSLog(
                outputTemplate: "[{Level}] [{SourceContext}] {Message:l}{NewLine:l}{Exception:l}"
            );
        }
    }
}
#endif