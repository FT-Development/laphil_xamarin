using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;

namespace HollywoodBowl.Droid
{
    public class AboutTheArtistView : Fragment
    {
        private Context mContext;
        private TextView lblArtistName, lblArtistDesc, lblHeader;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //LayoutInflaterCompat.SetFactory2(inflater, new FontUtils());
            inflater.Factory = new FontUtils();
            View view = inflater.Inflate(Resource.Layout.AboutTheArtistView, container, false);
            mContext = this.Activity;
            var btnBack = view.FindViewById<ImageView>(Resource.Id.btnBack);
            InitView(view,mContext);
            btnBack.Click += (sender, e) =>
            {
                FragmentManager.PopBackStack();
            };

            return view.RootView;
        }

        private void InitView(View view,Context context){
            lblArtistName = view.FindViewById<TextView>(Resource.Id.lblArtistName);
            lblArtistDesc = view.FindViewById<TextView>(Resource.Id.lblArtistDesc);
            lblHeader = view.FindViewById<TextView>(Resource.Id.lblHeader);
            lblArtistName.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblArtistDesc.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblHeader.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
        }

    }
}
