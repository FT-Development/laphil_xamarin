#if __ANDROID__
using System;
using Android.Content;
using Android.Text.Format;

namespace LAPhil.Platform
{
    public class TimeService
    {
        public bool Uses24HourTime { get; private set; } = false;
       
        public TimeService(Context context)
        {
            DetectTimeFormat();
          
        }

        public void DetectTimeFormat()
        {
            //Uses24HourTime = DateFormat.Is24HourFormat(context);
           // Uses24HourTime = formatString.Contains("a") ? false : true;
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

