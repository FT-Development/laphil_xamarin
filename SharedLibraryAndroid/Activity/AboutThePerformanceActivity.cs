
using Android.Content.PM;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Widget;
using LAPhil.Events;
using System;
using Android.Text;
using Com.Bumptech.Glide;
using LAPhil.Droid;

namespace SharedLibraryAndroid                 
{    
    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutThePerformanceActivity : Activity
    {
        private Context mContext;
        private TextView lblPerformanceName, lblPerformanceDesc, lblHeader,lblSeriesName;
        public Event selectEvent { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AboutThePerformanceView);
            mContext = this;
            var btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            InitView(mContext);
            selectEvent = UserModel.Instance.SelectedEvent;
           
            if (((int)Build.VERSION.SdkInt) >= 24)
            {
                lblPerformanceName.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Name"), FromHtmlOptions.ModeLegacy);
                lblPerformanceDesc.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Description"), FromHtmlOptions.ModeLegacy);
            }
            else
            {
                lblPerformanceName.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Name"));
                lblPerformanceDesc.TextFormatted = Html.FromHtml(Intent.GetStringExtra("Description"));
            }

            if (selectEvent.Series != null && string.IsNullOrEmpty(selectEvent.Series.WebUrl) == false)
            {
                lblSeriesName.Visibility = Android.Views.ViewStates.Visible;
                lblSeriesName.Text = "PART OF " + selectEvent.Series.Name;

            }else{
                
                lblSeriesName.Visibility = Android.Views.ViewStates.Gone;
            }
            lblSeriesName.Click += delegate
            {
                string newUrl = selectEvent.Series.WebUrl;
                var uri = Android.Net.Uri.Parse(newUrl);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);

            };
            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

        }

        private void InitView(Context context)
        {
            lblPerformanceName = FindViewById<TextView>(Resource.Id.lblPerformanceName);
            lblPerformanceDesc = FindViewById<TextView>(Resource.Id.lblPerformanceDesc);
            lblSeriesName = FindViewById<TextView>(Resource.Id.lblSeriesName);
            lblHeader = FindViewById<TextView>(Resource.Id.lblHeader);
            lblPerformanceName.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblPerformanceDesc.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblHeader.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblSeriesName.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
        }
    }
}
