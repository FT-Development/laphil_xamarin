using System;
namespace LAPhil.Analytics
{
    public class AnalyticsService
    {
        public readonly IAnalyticsDriver Driver;
        public AnalyticsService(IAnalyticsDriver driver)
        {
            Driver = driver;
        }

        public void TrackView(string viewLabel)
        {
            Driver.TrackView(viewLabel);
        }

        public void TrackEvent()
        {
            Driver.TrackEvent();
        }

    }
}
