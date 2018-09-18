using Foundation;
using System;
using System.Collections.Generic;
using System.Text;

//namespace GimbalSDK.iOS.Sample
namespace LAPhil.iOS
{
    //logs visits, locations, beacons for gimbal
    class GimbalPlaceManagerDelegate : GimbalFramework.GMBLPlaceManagerDelegate
    {
        string kSource = @"Gimbal";

        void ReportPlaceEventToAnalytics(GimbalFramework.GMBLPlace place, UrbanAirship.UABoundaryEvent boundaryEvent)
        {
            var customBoundaryEvent = UrbanAirship.UARegionEvent.RegionEvent(place.Identifier, kSource, boundaryEvent);
            UrbanAirship.UAirship.Analytics().AddEvent(customBoundaryEvent);
        }

        public override void DidBeginVisit(GimbalFramework.GMBLPlaceManager manager, GimbalFramework.GMBLVisit visit)
        {
            Console.WriteLine("Adapter DidBeginVisit: " + visit.Place.Description);
            this.ReportPlaceEventToAnalytics(visit.Place, UrbanAirship.UABoundaryEvent.Enter);
        }

        public override void DidEndVisit(GimbalFramework.GMBLPlaceManager manager, GimbalFramework.GMBLVisit visit)
        {
            Console.WriteLine("Adapter DidEndVisit: " + visit.Place.Description);
            this.ReportPlaceEventToAnalytics(visit.Place, UrbanAirship.UABoundaryEvent.Exit);
        }

        public override void DidDetectLocation(GimbalFramework.GMBLPlaceManager manager, CoreLocation.CLLocation location)
        {
            Console.WriteLine("Adapter DidDetectLocation: " + location.Coordinate.Latitude + " " + location.Coordinate.Longitude);
        }

        public override void DidReceiveBeaconSighting(GimbalFramework.GMBLPlaceManager manager, GimbalFramework.GMBLBeaconSighting sighting, NSObject[] visits)
        {
            Console.WriteLine("Adapter DidReceiveBeaconSighting: " + sighting.Beacon.Name);
        }
    }
}
