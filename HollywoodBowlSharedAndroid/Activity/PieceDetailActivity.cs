using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using LAPhil.Application;
using LAPhil.Urls;
using LAPhil.User;
using Android.Content.PM;
using Android.Text;

namespace HollywoodBowl.Droid
{
   [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PieceDetailActivity : Activity
    {
        private Context mContext;
        private TextView lblPerformanceName, lblPerformanceDesc, lblHeader, lblcomposerName, lblcomposerTitle, lblPerformanceDescTitle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PieceDetailLayout);
            mContext = this;
            var btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            InitView(mContext);

           // lblPerformanceName.Text = Intent.GetStringExtra("Name");
            lblcomposerName.Text = Intent.GetStringExtra("ComposerName");
            //lblPerformanceDesc.Text = Arguments.GetString("Description");
            if (((int)Build.VERSION.SdkInt) >= 24)
            {
                lblPerformanceDesc.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Description"), FromHtmlOptions.ModeLegacy);
                lblPerformanceName.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Name"), FromHtmlOptions.ModeLegacy);
            }
            else
            {
                lblPerformanceDesc.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Description"));
                lblPerformanceName.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Name"));
            }


            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

        }
        private void InitView(Context mContext)
        {
            lblHeader = FindViewById<TextView>(Resource.Id.lblHeader);

            lblPerformanceName = FindViewById<TextView>(Resource.Id.lblPerformanceName);
            lblPerformanceDesc = FindViewById<TextView>(Resource.Id.lblPerformanceDesc);

            lblcomposerTitle = FindViewById<TextView>(Resource.Id.lblcomposerTitle);
            lblcomposerName = FindViewById<TextView>(Resource.Id.lblcomposerName);
            lblPerformanceDescTitle = FindViewById<TextView>(Resource.Id.lblPerformanceDescTitle);

            lblcomposerTitle.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblcomposerName.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblPerformanceDescTitle.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblPerformanceName.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblPerformanceDesc.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblHeader.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
        }

    }
}
