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
using Com.Gimbal.Android;
using Android.Locations;

namespace LAPhil.Droid
{
    class GimbalPlaceEventListener : PlaceEventListener
    {

        private static String SOURCE = "Gimbal";
        private static String TAG = "GimbalPlaceEventListener";

        public GimbalPlaceEventListener()
        {
            Log.Info(TAG, "GimbalPlaceEventListener Init ");
        }

        public override void OnVisitStart(Visit visit)
        {
            Log.Info(TAG, "OnVisitStart " + visit.Place.Name);//TODO
            RegionEvent enter = new RegionEvent(visit.Place.Identifier, SOURCE, RegionEvent.BoundaryEventEnter);
            UrbanAirship.UAirship.Shared().Analytics.AddEvent(enter);
        }

        public override void OnVisitEnd(Visit visit)
        {
            Log.Info(TAG, "OnVisitEnd " + visit.Place.Name);
            RegionEvent exit = new RegionEvent(visit.Place.Identifier, SOURCE, RegionEvent.BoundaryEventExit);
            UrbanAirship.UAirship.Shared().Analytics.AddEvent(exit);
        }

        public override void LocationDetected(Location location)
        {
            base.LocationDetected(location);
            Log.Info(TAG, "LocationDetected " + location.Latitude + ", " + location.Longitude);
        }
    }
}