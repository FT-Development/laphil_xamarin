#if __ANDROID__
using System;
using Android.App;


namespace LAPhil.Platform
{
    public class CurrentActivityService
    {
        public Activity Activity { get; internal set; }

        public CurrentActivityService()
        {
        }
    }
}
#endif
