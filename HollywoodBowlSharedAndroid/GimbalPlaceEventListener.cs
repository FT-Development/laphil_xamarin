using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using UrbanAirship.Location;

namespace HollywoodBowl.Droid
{
    class GimbalPlaceEventListener : Com.Gimbal.Android.PlaceEventListener
    {

        private static String TAG = "GimbalPlaceEventListener";
        private static String SOURCE = "Gimbal";

        public GimbalPlaceEventListener()
        {
            Log.Info(TAG, "GimbalPlaceEventListener Init ");
        }

        public override void OnBeaconSighting(Com.Gimbal.Android.BeaconSighting p0, System.Collections.Generic.IList<Com.Gimbal.Android.Visit> p1)
        {
            if (p1 != null && p1.Any())
            {
                foreach (var p in p1)
                {
                    Log.Info(TAG, "OnBeaconSighting Entered place: " + p.Place.Name);
                }
            }
        }

        public override void OnVisitStart(Com.Gimbal.Android.Visit visit)
        {
            Log.Info(TAG, "OnVisitStart Entered place: " + visit.Place.Name);
            RegionEvent enter = new RegionEvent(visit.Place.Identifier, SOURCE, RegionEvent.BoundaryEventEnter);
            UrbanAirship.UAirship.Shared().Analytics.AddEvent(enter);
        }

        public override void OnVisitEnd(Com.Gimbal.Android.Visit visit)
        {
            Log.Info(TAG, "OnVisitEnd Exited place: " + visit.Place.Name);
            RegionEvent exit = new RegionEvent(visit.Place.Identifier, SOURCE, RegionEvent.BoundaryEventExit);
            UrbanAirship.UAirship.Shared().Analytics.AddEvent(exit);
        }

    }
}