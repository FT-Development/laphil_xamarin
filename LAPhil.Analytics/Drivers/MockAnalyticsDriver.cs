using System;
using LAPhil.Application;
using LAPhil.Logging;


namespace LAPhil.Analytics
{
    public class MockAnalyticsDriver: IAnalyticsDriver
    {
        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<MockAnalyticsDriver>();

        public MockAnalyticsDriver()
        {
            Log.Debug("Initialized Mock Analytics Driver");
        }

        public void TrackEvent()
        {
            Log.Debug("Tracking Event...");
        }

        public void TrackView(string viewLabel)
        {
            Log.Debug("Tracking View... '{ViewLabel}'", viewLabel);
        }
    }
}
