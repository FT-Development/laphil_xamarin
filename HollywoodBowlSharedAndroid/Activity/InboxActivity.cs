using System;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using UrbanAirship.MessageCenter;
using UrbanAirship.RichPush;
using UrbanAirship;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)] 
     public class InboxActivity : AppCompatActivity
    {
        private Context mContext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InboxActivityLayout);
            mContext = this;
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            var header = FindViewById<TextView>(Resource.Id.header);
            var btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            header.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            
            MessageCenterFragment fragment = new MessageCenterFragment();
            SupportFragmentManager.BeginTransaction()
                           .Replace(Resource.Id.content_frame, fragment, "content_frag")
                           .Commit();
            
            btnBack.Click += (s, e) =>
            {
                Finish();
            };
        }
    }
}
