using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using LAPhil.Droid;
using LAPhil.Urls;

namespace SharedLibraryAndroid
{
    public class WhenHereView : Fragment
    {
        private RelativeLayout btnNearByDining, btnSeatingChart, btnAccessibility;
        private TextView lblName, lblDesc;
        private Context mContext;
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.WhenHereView, container, false);
            mContext = this.Activity;

            InitView(v,mContext);

            btnNearByDining.Click += delegate
            {
                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebDining);
                args.PutString("header", "NearBy Dining");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            btnSeatingChart.Click += delegate
            {
                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebChart);
                args.PutString("header", "Seating Chart");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            btnAccessibility.Click += delegate
            {
                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebAccessibility);
                args.PutString("header", "Accessibility");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return v.RootView;
        }
        public void InitView(View v,Context mContext)
        {
             btnNearByDining = v.FindViewById<RelativeLayout>(Resource.Id.btnNearByDining);
             btnSeatingChart = v.FindViewById<RelativeLayout>(Resource.Id.btnSteatingChart);
             btnAccessibility = v.FindViewById<RelativeLayout>(Resource.Id.btnAccessibility);
             lblName = v.FindViewById<TextView>(Resource.Id.lblName);
             lblDesc = v.FindViewById<TextView>(Resource.Id.lblDesc);
             lblName.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
             lblDesc.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);

            var lblheadingText = v.FindViewById<TextView>(Resource.Id.headingText);
            var lblnearText = v.FindViewById<TextView>(Resource.Id.nearbyText);
            var lblseatibgText = v.FindViewById<TextView>(Resource.Id.seatingChartText);
            var lblAccessibilityText = v.FindViewById<TextView>(Resource.Id.accessibilityText);

            lblheadingText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblnearText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblseatibgText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblAccessibilityText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

             
        }
    }
}
