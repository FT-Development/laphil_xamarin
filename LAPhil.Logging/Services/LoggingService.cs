using System;
using Serilog;


namespace LAPhil.Logging
{
    
    public interface ILog
    {
        void Debug(string message);
        void Debug<T>(string message, T propertyValue);
        void Debug<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1);
        void Debug<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Info(string message);
        void Info<T>(string message, T propertyValue);
        void Info<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1);
        void Info<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Warn(string message);
        void Warn<T>(string message, T propertyValue);
        void Warn<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1);
        void Warn<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Error(string message);
        void Error(Exception ex, string message);
        void Error<T>(string message, T propertyValue);
        void Error<T>(Exception ex, string message, T propertyValue);
        void Error<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1);
        void Error<T0, T1>(Exception ex, string message, T0 propertyValue0, T1 propertyValue1);
        void Error<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Error<T0, T1, T2>(Exception ex, string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Error<T0, T1, T2, T3>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2, T3 propertyValue3);
        void Error<T0, T1, T2, T3>(Exception ex, string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2, T3 propertyValue3);
    }

    public class LoggingService
    {
        public LoggingService(IPlatformLogger platform)
        {

            Serilog.Log.Logger = platform.Configure(new LoggerConfiguration())
                    .MinimumLevel.Debug()
                    .CreateLogger();
        }

        public Logger<T> GetLogger<T>()
        {
            var logger = Serilog.Log.Logger.ForContext<T>();
            return new Logger<T>(logger);
        }

        public ILog GetLoggerA(Type type)
        {
            var logger = Serilog.Log.Logger.ForContext(type);
            return null;
        }

        public void Shutdown()
        {
            Serilog.Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// A wrapper over the actual logging service 
    /// to avoid a `using` of the underlying logging provider.
    /// </summary>
    public class Logger<TContext>: ILog
    {
        public ILogger Log { get; private set; }

        public Logger(ILogger log)
        {
            Log = log;
        }


        #region Debug
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Debug<T>(string message, T propertyValue)
        {
            Log.Debug(message, propertyValue);
        }

        public void Debug<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Debug(message, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Debug(message, propertyValue0, propertyValue1, propertyValue2);
        }
        #endregion

        #region Info
        public void Info(string message)
        {
            Log.Information(message);
        }

        public void Info<T>(string message, T propertyValue)
        {
            Log.Information(message, propertyValue);
        }

        public void Info<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Information(message, propertyValue0, propertyValue1);
        }

        public void Info<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Information(message, propertyValue0, propertyValue1, propertyValue2);
        }
        #endregion

        #region Warn
        public void Warn(string message)
        {
            Log.Warning(message);
        }

        public void Warn<T>(string message, T propertyValue)
        {
            Log.Warning(message, propertyValue);
        }

        public void Warn<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Warning(message, propertyValue0, propertyValue1);
        }

        public void Warn<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Warning(message, propertyValue0, propertyValue1, propertyValue2);
        }
        #endregion

        #region Error
        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            Log.Error(ex, message);
        }

        public void Error<T>(string message, T propertyValue)
        {
            Log.Error(message, propertyValue);
        }

        public void Error<T>(Exception ex, string message, T propertyValue)
        {
            Log.Error(ex, message, propertyValue);
        }

        public void Error<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Error(message, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1>(Exception ex, string message, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Error(ex, message, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Error(message, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error<T0, T1, T2>(Exception ex, string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Error(ex, message, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error<T0, T1, T2, T3>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2, T3 propertyValue3)
        {
            Log.Error(message, propertyValue0, propertyValue1, propertyValue2, propertyValue3);
        }

        public void Error<T0, T1, T2, T3>(Exception ex, string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2, T3 propertyValue3)
        {
            Log.Error(ex, message, propertyValue0, propertyValue1, propertyValue2, propertyValue3);
        }
        #endregion

    }
}
