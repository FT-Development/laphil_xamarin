#if __IOS__
using System;
using Foundation;
using Kochava;


namespace LAPhil.Analytics.Platform
{
    public class KochavaDriver: IAnalyticsDriver
    {
        public KochavaDriver(string appId)
        {
            var config = new NSMutableDictionary();
            config.Add(Kochava.Constants.kKVAParamAppGUIDStringKey, NSObject.FromObject(appId));

#if DEBUG
            config.Add(Kochava.Constants.kKVAParamLogLevelEnumKey, Kochava.Constants.kKVALogLevelEnumDebug);
#else
            config.Add(Kochava.Constants.kKVAParamLogLevelEnumKey, Kochava.Constants.kKVALogLevelEnumInfo);
#endif
            Kochava.Tracker.Shared.ConfigureWithParametersDictionary(parametersDictionary: config, callback: null);
        }

        public void TrackEvent()
        {
        
        }

        public void TrackView(string viewLabel)
        {
            return;
            var kochavaEvent = Kochava.Event.EventWithEventTypeEnum(Kochava.EventTypeEnum.View);
            //kochavaEvent

            Kochava.Tracker.Shared.SendEvent(kochavaEvent);
        }
    }
}
#endif