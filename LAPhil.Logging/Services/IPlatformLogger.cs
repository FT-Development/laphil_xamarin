
using Serilog;

namespace LAPhil.Logging
{
    public interface IPlatformLogger
    {
        LoggerConfiguration Configure(LoggerConfiguration config);        
    }
}