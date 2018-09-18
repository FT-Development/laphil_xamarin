using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using LAPhil.Droid;
using Android.Text;

namespace SharedLibraryAndroid
{
    public class PieceDetailView : Fragment
    {
        private Context mContext;
        private TextView lblPerformanceName, lblPerformanceDesc, lblHeader,lblcomposerName,lblcomposerTitle,lblPerformanceDescTitle;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PieceDetailLayout, container, false);
            mContext = this.Activity;
            var btnBack = view.FindViewById<ImageView>(Resource.Id.btnBack);
            InitView(view, mContext);

            lblPerformanceName.Text = Arguments.GetString("Name");
            lblcomposerName.Text = Arguments.GetString("ComposerName");
            //lblPerformanceDesc.Text = Arguments.GetString("Description");
            if (((int)Build.VERSION.SdkInt) >= 24)
            {
                lblPerformanceDesc.TextFormatted = Html.FromHtml(Arguments.GetString("Description"), FromHtmlOptions.ModeLegacy);
            }
            else
            {
                lblPerformanceDesc.TextFormatted = Html.FromHtml(Arguments.GetString("Description"));
            }


            btnBack.Click += (sender, e) =>
            {
                FragmentManager.PopBackStack();
            };

            return view.RootView;
        }

        private void InitView(View view, Context mContext)
        {
            lblHeader = view.FindViewById<TextView>(Resource.Id.lblHeader);

            lblPerformanceName = view.FindViewById<TextView>(Resource.Id.lblPerformanceName);
            lblPerformanceDesc = view.FindViewById<TextView>(Resource.Id.lblPerformanceDesc);

            lblcomposerTitle = view.FindViewById<TextView>(Resource.Id.lblcomposerTitle);
            lblcomposerName = view.FindViewById<TextView>(Resource.Id.lblcomposerName);
            lblPerformanceDescTitle = view.FindViewById<TextView>(Resource.Id.lblPerformanceDescTitle);
           
            lblcomposerTitle.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblcomposerName.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblPerformanceDescTitle.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblPerformanceName.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblPerformanceDesc.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblHeader.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
        }

    }
}

