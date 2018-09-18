using Android.Content.PM;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using LAPhil.Application;
using System;
using LAPhil.Urls;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
 [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
  public class SupportUsActivity : Activity
    {
        
        LAPhilUrlService urlService = ServiceContainer.Resolve<LAPhilUrlService>();
        private RelativeLayout btnCentennial, btnMakeAGift, btnOtherWay, btnCorporateSpon;
        private TextView centennialText, makeAGiftText, corporateText, otherWaysText;
        private Context mContext;
        private TabBarView tabBarView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Support_us_view);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;
            var btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            InitView(mContext);

            btnCentennial.Click += delegate
            {
                CallWebPage(urlService.WebCentennial, "Centennial Campaign");
            };
            btnMakeAGift.Click += delegate
            {
                CallWebPage(urlService.WebMakeAGift, "Make a Gift");
            };
            btnCorporateSpon.Click += delegate
            {
                CallWebPage(urlService.WebCorporateSponsorship, "Corporate Sponsorship");
            };
            btnOtherWay.Click += delegate
            {
                CallWebPage(urlService.WebOtherWaysToSupport, "Other Ways to Support");
            };
            btnBack.Click += (sender, e) =>
            {
                UserModel.Instance.SelectedTab = "More";
                Intent intent = new Intent(this, typeof(MoreActivity));
                StartActivity(intent);
                Finish();
            };
        }
        private void InitView(Context context)
        {
            btnCentennial = FindViewById<RelativeLayout>(Resource.Id.btnCentennial);
            btnMakeAGift = FindViewById<RelativeLayout>(Resource.Id.btnMake);
            btnCorporateSpon = FindViewById<RelativeLayout>(Resource.Id.btnCorporate);
            btnOtherWay = FindViewById<RelativeLayout>(Resource.Id.btnOtherWays);

            centennialText = FindViewById<TextView>(Resource.Id.centennialText);
            makeAGiftText = FindViewById<TextView>(Resource.Id.makeText);
            corporateText = FindViewById<TextView>(Resource.Id.corporateText);
            otherWaysText = FindViewById<TextView>(Resource.Id.otherWaysText);

            var lblHeaderTextView = FindViewById<TextView>(Resource.Id.headerTitle);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);

            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(context);
            lytCustomBottom.AddView(tabBarView);
            centennialText.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            makeAGiftText.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            corporateText.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            otherWaysText.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);

        }

        private void CallWebPage(String url, String headerText)
        {
            UserModel.Instance.isFromMore = false;
            Intent intent= new Intent(this, typeof(WebViewActivity));
            intent.PutExtra("url", url);
            intent.PutExtra("header", headerText);
            StartActivity(intent);
            Finish();
        }
		public override void OnBackPressed()
		{
			base.OnBackPressed();
            UserModel.Instance.SelectedTab = "More";
            Intent intent = new Intent(this, typeof(MoreActivity));
            StartActivity(intent);
            Finish();
		}
	}
}
