using System;
using Android.Content;
using Android.Runtime;
using UrbanAirship;

namespace HollywoodBowl.Droid
{
    [Register("HollywoodBowl.Droid.InboxMessageAutopilot")]
 //   [Register("com.hollywoodbowl.bowlapp.InboxMessageAutopilot")]
    public class InboxMessageAutopilot : Autopilot
    {
        public override void OnAirshipReady(UAirship airship)
        {
            // perform any post takeOff airship customizations
            airship.PushManager.PushEnabled = true;
            airship.PushManager.UserNotificationsEnabled = true;

        }

        public override AirshipConfigOptions CreateAirshipConfigOptions(Context context)
        {
            AirshipConfigOptions options = new AirshipConfigOptions.Builder()
                 .SetInProduction(true)
                 .SetDevelopmentAppKey("IUNz7rBvRXiNRPstvGxpGg")
                 .SetDevelopmentAppSecret("wawJWxFwTOW3n5J5PMUgcg")
                 .SetProductionAppKey("IUNz7rBvRXiNRPstvGxpGg")
                 .SetProductionAppSecret("wawJWxFwTOW3n5J5PMUgcg")      
                 .SetFcmSenderId("55941271460")
                 .Build();
          
            return  options;
        }
    }
}
//productionAppKey = IUNz7rBvRXiNRPstvGxpGg
//productionAppSecret = wawJWxFwTOW3n5J5PMUgcg