using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace HollywoodBowl.Droid
{
    public class WhenHereView : Fragment
    {
        private RelativeLayout btnNearByDining, btnSeatingChart, btnAccessibility;
        private TextView lblName, lblDesc;
        private Context mContext;
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
                args.PutString("url", "http://laphil-staging.herokuapp.com/plan-your-visit/when-youre-here/dining/?edit&language=en");
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
                args.PutString("url", "http://laphil-staging.herokuapp.com/plan-your-visit/when-youre-here/seating-chart/?edit&language=en");
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
                args.PutString("url", "http://laphil-staging.herokuapp.com/plan-your-visit/when-youre-here/accessibility-info/?edit&language=en");
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
        }
    }
}
