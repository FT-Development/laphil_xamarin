using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using LAPhil.Application;
using LAPhil.Urls;

namespace HollywoodBowl.Droid
{
    public class SupportUsView : Fragment
    {
        LAPhilUrlService urlService = ServiceContainer.Resolve<LAPhilUrlService>();
        private RelativeLayout btnCentennial, btnMakeAGift, btnOtherWay, btnCorporateSpon;
        private TextView centennialText, makeAGiftText, corporateText, otherWaysText;
        private Context mContext;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Support_us_view, container, false);
            mContext = this.Activity;
            var btnBack = view.FindViewById<ImageView>(Resource.Id.btnBack);
            InitView(mContext, view);

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
                FragmentManager.PopBackStack();
            };
            return view;
        }

        private void InitView(Context mContext, View view)
        {
            btnCentennial = view.FindViewById<RelativeLayout>(Resource.Id.btnCentennial);
            btnMakeAGift = view.FindViewById<RelativeLayout>(Resource.Id.btnMake);
            btnCorporateSpon = view.FindViewById<RelativeLayout>(Resource.Id.btnCorporate);
            btnOtherWay = view.FindViewById<RelativeLayout>(Resource.Id.btnOtherWays);

            centennialText = view.FindViewById<TextView>(Resource.Id.centennialText);
            makeAGiftText = view.FindViewById<TextView>(Resource.Id.makeText);
            corporateText = view.FindViewById<TextView>(Resource.Id.corporateText);
            otherWaysText = view.FindViewById<TextView>(Resource.Id.otherWaysText);

            var lblHeaderTextView = view.FindViewById<TextView>(Resource.Id.headerTitle);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            centennialText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            makeAGiftText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            corporateText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            otherWaysText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

        }

        private void CallWebPage(String url, String headerText)
        {
            WhenYouHereWebViews fragment = new WhenYouHereWebViews();
            Bundle args = new Bundle();
            args.PutString("url", url);
            args.PutString("header", headerText);
            fragment.Arguments = args;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }
    }
}
