using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using LAPhil.Application;
using LAPhil.Droid;
using LAPhil.Urls;
using LAPhil.User;

namespace SharedLibraryAndroid
{
    public class MoreView : Fragment
    {
        private Context mContext;
        private TextView logoutTextView,faqText,supportUsText,shareAppText,gettingTextV,myAccountTextV,seatingchartTextV;
        private RelativeLayout btnFAQ, btnSupportUs, btnLogout, btnGettingHere, btnShareApp,btnMyAccount,btnSeatingchart;
        LAPhilUrlService urlService = ServiceContainer.Resolve<LAPhilUrlService>();
        LoginService loginService = ServiceContainer.Resolve<LoginService>();
        private View myAccountViewLine;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View  view=inflater.Inflate(Resource.Layout.MoreView, container, false);
            mContext = this.Activity;
            InitView(mContext, view);
            ShowHideView();
           
            btnLogout.Click += Button2_Click;

            btnGettingHere.Click += delegate 
            {

                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebGettingHere);
                args.PutString("header", "Getting Here");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();

                //GettingHereView fragment = new GettingHereView();
                //var transaction = FragmentManager.BeginTransaction();
                //transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                //transaction.AddToBackStack(null);
                //transaction.Commit();
            };


            btnMyAccount.Click += delegate
            {
                string newUrl = urlService.WebMyAccountDetails;
                var uri = Android.Net.Uri.Parse(newUrl);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

            btnShareApp.Click += delegate
            {
                Share("Share LAPhil App",urlService.AppStore);
            };

            btnSupportUs.Click += delegate
            {
                SupportUsView fragment = new SupportUsView();
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };
            btnFAQ.Click += delegate
            {
                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebFAQ);
                args.PutString("header", "FAQ");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };
            btnSeatingchart.Click += delegate
            {
                WhenYouHereWebViews fragment = new WhenYouHereWebViews();
                Bundle args = new Bundle();
                args.PutString("url", urlService.WebSeatingChart);
                args.PutString("header", "Seating Chart");
                fragment.Arguments = args;
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }

        private void ShowHideView(){
            if (!Utility.GetBooleanSharedPreference("isLogin"))
            {
                logoutTextView.Text = "LOG IN";
                myAccountViewLine.Visibility = ViewStates.Gone;
                btnMyAccount.Visibility = ViewStates.Gone;
                logoutTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            }
            else
            {
                logoutTextView.Text = "LOG OUT";
                myAccountViewLine.Visibility = ViewStates.Visible;
                btnMyAccount.Visibility = ViewStates.Visible;
                logoutTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                myAccountTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            }
        }

        private void InitView(Context mContext, View view)
        {
            btnGettingHere = view.FindViewById<RelativeLayout>(Resource.Id.btnGettingHere);
            btnLogout = view.FindViewById<RelativeLayout>(Resource.Id.btnLogout);
            btnSupportUs = view.FindViewById<RelativeLayout>(Resource.Id.btnSupportUs);
            btnFAQ = view.FindViewById<RelativeLayout>(Resource.Id.btnFAQ);
            btnShareApp = view.FindViewById<RelativeLayout>(Resource.Id.btnShareApp);
            btnMyAccount = view.FindViewById<RelativeLayout>(Resource.Id.btnMyAccount);
            myAccountViewLine = view.FindViewById<View>(Resource.Id.myAccountView);
            btnSeatingchart = view.FindViewById<RelativeLayout>(Resource.Id.btnSeatingchart);

            logoutTextView = view.FindViewById<TextView>(Resource.Id.logoutTextView);
            supportUsText = view.FindViewById<TextView>(Resource.Id.supportTextV);
            shareAppText = view.FindViewById<TextView>(Resource.Id.shareTextV);
            faqText = view.FindViewById<TextView>(Resource.Id.faqTextV);
            gettingTextV = view.FindViewById<TextView>(Resource.Id.gettingTextV);
            myAccountTextV = view.FindViewById<TextView>(Resource.Id.myAccountTextV);
            seatingchartTextV = view.FindViewById<TextView>(Resource.Id.SeatingTextV);

            seatingchartTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            gettingTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            shareAppText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            faqText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            logoutTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            supportUsText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            myAccountTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            //logoutTextView = view.FindViewById<TextView>(Resource.Id.logoutTextView);
           
        }
        private  void Button2_Click(object sender, EventArgs e)  
        { 
            if (!Utility.GetBooleanSharedPreference("isLogin")) {
                Intent intent = new Intent(mContext, typeof(LoginActivity));
                StartActivity(intent);
            }
            else
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this.Activity);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Log out");
                alert.SetMessage("Are you sure you want to logout?");
                alert.SetButton("Yes", (c, ev) =>
                {
                    Utility.SetBooleanSharedPreference("isLogin", false);
                    var islogin = loginService.Logout();
                    UserModel.Instance.tickets = null;
                    UserModel.Instance.ticketsEvent=null;
                    ShowHideView();
                });
                alert.SetButton2("No", (c, ev) => { });
                alert.Show();
            }
         } 

        public void Share(string title, string content)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                return;

            Bitmap b = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.Icon);

            var tempFilename = "AppIcon.png";
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, tempFilename);
            using (var os = new FileStream(filePath, FileMode.Create))
            {
                b.Compress(Bitmap.CompressFormat.Png, 60, os);
            }
            b.Dispose();

            var imageUri = Android.Net.Uri.Parse($"file://{sdCardPath}/{tempFilename}");
            var sharingIntent = new Intent();
            sharingIntent.SetAction(Intent.ActionSend);
            sharingIntent.SetType("image/*");
            sharingIntent.PutExtra(Intent.ExtraText, content);
            sharingIntent.PutExtra(Intent.ExtraStream, imageUri);
            sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            StartActivity(Intent.CreateChooser(sharingIntent, title));
        }

    }
}
