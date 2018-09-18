using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Webkit;
using System.Threading.Tasks;

namespace 
{

    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class InteractiveMapActivity : Activity
    {
        private Context mContext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InteractiveMapLayout);
            mContext = this;
            TextView lblHeaderTextView = FindViewById<TextView>(Resource.Id.lblHeaderTextView);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblHeaderTextView.Text = "Map";
            ImageView btnBack = FindViewById<ImageView>(Resource.Id.btnBack);

            btnBack.Click += (sender, e) =>
            {
                Finish();
            };
        }
    }

}
