using System;
namespace LAPhil.Analytics
{
    public interface IAnalyticsDriver
    {
        void TrackView(string viewLabel);
        void TrackEvent();
    }
}
