using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using LAPhil.Droid;
using LAPhil.Urls;

namespace SharedLibraryAndroid
{
    public class GettingHereView : Fragment
    {
        private Context mContext;
        private RelativeLayout btnParking, btnBus, btnMetro;
        private ImageView btnBack;
        private TextView LocationName, LocationAddress, btnDirection, DescHeading, Description;
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view= inflater.Inflate(Resource.Layout.GettingHereView, container, false);
        
            mContext = this.Activity;
            InitView(view,mContext);
           
            btnParking.Click += delegate
            {
                Intent intent = new Intent(this.Activity, typeof(ParkingView));
                mContext.StartActivity(intent);
            };

            btnBus.Click += delegate
            {
                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebMetroBus);
                args.PutString("header", "Bus");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();       
            };

            btnMetro.Click += delegate
            {
                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebMetroBus);
                args.PutString("header", "Metro");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            btnBack.Click += (sender,e) => 
            {
                FragmentManager.PopBackStack();
            };
            return view.RootView;
        }

      
       
        public override void OnPause()
        {
            base.OnPause();
            //if (_mapFragment != null)
            //{
            //    FragmentManager.BeginTransaction().Remove(_mapFragment).Commit();
            //}
        }
        private void InitView(View view,Context context)
        {
            btnParking = view.FindViewById<RelativeLayout>(Resource.Id.btnParking);
            btnBus = view.FindViewById<RelativeLayout>(Resource.Id.btnBus);
            btnMetro = view.FindViewById<RelativeLayout>(Resource.Id.btnMetro);
            btnBack = view.FindViewById<ImageView>(Resource.Id.back_btn);

            view.FindViewById<TextView>(Resource.Id.textParking).SetTypeface(Utility.MediumTypeface(context),Android.Graphics.TypefaceStyle.Normal);
            view.FindViewById<TextView>(Resource.Id.textBus).SetTypeface(Utility.MediumTypeface(context), Android.Graphics.TypefaceStyle.Normal);
            view.FindViewById<TextView>(Resource.Id.textMetro).SetTypeface(Utility.MediumTypeface(context), Android.Graphics.TypefaceStyle.Normal);
           
            Description = view.FindViewById<TextView>(Resource.Id.lblDescription);
            Description.SetTypeface(Utility.RegularTypeface(context), Android.Graphics.TypefaceStyle.Normal);
           
            DescHeading = view.FindViewById<TextView>(Resource.Id.lblHeading);
            DescHeading.SetTypeface(Utility.BoldTypeface(context), Android.Graphics.TypefaceStyle.Bold);

            LocationAddress= view.FindViewById<TextView>(Resource.Id.lblLocationAdd);
            LocationAddress.SetTypeface(Utility.MediumTypeface(context), Android.Graphics.TypefaceStyle.Normal);

            LocationName=  view.FindViewById<TextView>(Resource.Id.lblVanueName);
            LocationName.SetTypeface(Utility.BoldTypeface(context), Android.Graphics.TypefaceStyle.Bold);
           
            btnDirection= view.FindViewById<TextView>(Resource.Id.btnGetDirection);
            btnDirection.SetTypeface(Utility.MediumTypeface(context), Android.Graphics.TypefaceStyle.Normal);
        }
    }


}
