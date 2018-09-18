#if __IOS__
using System;
using Foundation;



namespace LAPhil.Platform
{
    public class TimeService
    {
        public bool Uses24HourTime { get; private set; } = false;

        public TimeService()
        {
            DetectTimeFormat();
        }

        public void DetectTimeFormat()
        {
            var formatString = NSDateFormatter.GetDateFormatFromTemplate("j", options: 0, locale: NSLocale.CurrentLocale);
            Uses24HourTime = formatString.Contains("a") ? false : true;
        }

        public string TimeFormat
        {
            get 
            {
                if (Uses24HourTime)
                    return "H:mm";

                return "h:mm tt";
            }
        }
    }
}
#endif

