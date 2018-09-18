using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Android.Widget;
using Android.Support.V7.App;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "Parking", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ParkingView : AppCompatActivity
    {
        private Context mContext;
        private TextView parkingDescText, header, btnViewLargerMap, descHeader;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ParkingView);
            mContext = this;
            InitView(mContext);
            var btnBack = FindViewById<ImageView>(Resource.Id.back_btn);

            btnBack.Click += (sender, e) =>
            {
                Finish();
            };
        }
        private void InitView(Context mContext)
        {
            parkingDescText = FindViewById<TextView>(Resource.Id.parkingDescText);
            header = FindViewById<TextView>(Resource.Id.header);
            btnViewLargerMap = FindViewById<TextView>(Resource.Id.btnViewLargerMap);
            descHeader = FindViewById<TextView>(Resource.Id.descHeader);
            descHeader.SetTypeface(Utility.BoldTypeface(mContext),Android.Graphics.TypefaceStyle.Bold);
            btnViewLargerMap.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            header.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            parkingDescText.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);

        }
    }
}
