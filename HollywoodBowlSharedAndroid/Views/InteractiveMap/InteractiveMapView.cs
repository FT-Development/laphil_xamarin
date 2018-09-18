using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Graphics;


namespace HollywoodBowl.Droid
{
    public class InteractiveMapView : Fragment
    {
        private Context mContext;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.InteractiveMapLayout, container, false);
            mContext = this.Activity;
            TextView lblHeaderTextView = view.FindViewById<TextView>(Resource.Id.lblHeaderTextView);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblHeaderTextView.Text = "Map";
            ImageView btnBack = view.FindViewById<ImageView>(Resource.Id.btnBack);
     
            btnBack.Click += (sender, e) =>
            {
                FragmentManager.PopBackStack();
            };

            return view.RootView;

        }
    }
}
