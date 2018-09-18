#if __ANDROID__
using System;
using Com.Kochava.Base;


namespace LAPhil.Analytics.Platform
{
    public class KochavaDriver : IAnalyticsDriver
    {
        public KochavaDriver(string appId)
        {
            var config = new Tracker.Configuration();
            config.SetAppGuid(appId);
#if DEBUG
            config.SetLogLevel(Tracker.LogLevelDebug);
#else
            config.SetLogLevel(Tracker.LogLevelInfo);
#endif
            Tracker.Configure(config);
        }

        public void TrackEvent()
        {

        }

        public void TrackView(string viewLabel)
        {
            return;
            Tracker.SendEvent(new Tracker.Event(Tracker.EventTypePurchase)
           .SetName("Widget")
           .SetCurrency("USD")
           .SetPrice(0.99));
        }
    }
}
#endif