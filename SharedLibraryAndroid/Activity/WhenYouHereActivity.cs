using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using LAPhil.Droid;
using LAPhil.Urls;

namespace SharedLibraryAndroid
{
  
    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
       public class WhenYouHereActivity : Activity
        {
        private RelativeLayout btnNearByDining, btnSeatingChart, btnAccessibility;
        private TextView lblName, lblDesc;
        private Context mContext;
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WhenHereView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;

            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            var tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);

            InitView(mContext);
            btnNearByDining.Click += delegate
            {
                CallWebPage(urlService.WebDining, "Nearby Dining");

            };

            btnSeatingChart.Click += delegate
            {
                
                CallWebPage(urlService.WebChart, "Seating Chart");

            };

            btnAccessibility.Click += delegate
            {
              
                CallWebPage(urlService.WebAccessibility,"Accessibility");

            };
        }

        public void InitView(Context mContext)
        {
            btnNearByDining = FindViewById<RelativeLayout>(Resource.Id.btnNearByDining);
            btnSeatingChart = FindViewById<RelativeLayout>(Resource.Id.btnSteatingChart);
            btnAccessibility = FindViewById<RelativeLayout>(Resource.Id.btnAccessibility);
            lblName = FindViewById<TextView>(Resource.Id.lblName);
            lblDesc = FindViewById<TextView>(Resource.Id.lblDesc);
            lblName.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblDesc.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);

            var lblheadingText = FindViewById<TextView>(Resource.Id.headingText);
            var lblnearText = FindViewById<TextView>(Resource.Id.nearbyText);
            var lblseatibgText = FindViewById<TextView>(Resource.Id.seatingChartText);
            var lblAccessibilityText = FindViewById<TextView>(Resource.Id.accessibilityText);

            lblheadingText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblnearText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblseatibgText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblAccessibilityText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

        }
        private void CallWebPage(String url, String headerText)
        {
            Intent intent = new Intent(this, typeof(WhenYouWebViewActivity));
            intent.PutExtra("url", url);
            intent.PutExtra("header", headerText);
            StartActivity(intent);
            Finish();
        }
    }
}
