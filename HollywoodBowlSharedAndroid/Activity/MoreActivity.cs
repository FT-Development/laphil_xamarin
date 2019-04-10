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
using System.Threading.Tasks;


namespace HollywoodBowl.Droid
{
    [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MoreActivity : Activity
    {  
        private Context mContext;
        private TextView logoutTextView, faqText,inboxTextV, supportUsText, shareAppText, gettingTextV, myAccountTextV, seatingchartTextV, mapTextV, storeText, orderingText, privacyText, aboutUsText;
        private RelativeLayout btnFAQ, btnSupportUs,btnInbox, btnLogout, btnGettingHere, btnShareApp, btnMyAccount, btnSeatingchart, btnMapImageView, btnStore, btnMobileOrdering, btnPrivacy, btnAboutUs;
        private View myAccountViewLine;
        LAPhilUrlService urlService = ServiceContainer.Resolve<LAPhilUrlService>();
        LoginService loginService = ServiceContainer.Resolve<LoginService>();

        private TabBarView tabBarView;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MoreView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;
            InitView(mContext);
            ShowHideView();

            btnLogout.Click += Button2_Click;

            btnGettingHere.Click += delegate
            {
                CallWebPage(urlService.WebGettingHere, "Getting Here");
            };
            btnMyAccount.Click += delegate
            {
                string newUrl = urlService.WebMyAccountDetails;
                var uri = Android.Net.Uri.Parse(newUrl);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };
            btnSupportUs.Click += delegate
            {
                StartActivity(new Intent(this, typeof(SupportUsActivity)));
                Finish();
            };
            btnShareApp.Click += delegate
            {
                Share("Share HollywoodBowl App", urlService.AppStore);
            };
            btnFAQ.Click += delegate
            {
              
                CallWebPage(urlService.WebFAQ,"FAQ");

            };
            btnSeatingchart.Click += delegate
            {
                CallWebPage(urlService.WebSeatingChart,"Seating Chart");
            };

            btnMapImageView.Click += delegate 
            {
                Intent intent = new Intent(this, typeof(InteractiveMapActivity));
                StartActivity(intent);
            };
            btnInbox.Click += delegate
            {
                Intent intent = new Intent(this, typeof(InboxActivity));
                StartActivity(intent);
            };
            btnStore.Click += delegate
            {
                CallWebPage(urlService.BowlStore, "The Bowl Store");
            };
            btnMobileOrdering.Click += delegate
            {
                CallWebPage(urlService.MobileOrdering, "Mobile Ordering", true);
            };

            btnPrivacy.Click += delegate
            {
                CallWebPage(urlService.PrivacyPolicy, "Privacy Policy", IsPrivacyPolicy: true);
            };

            btnAboutUs.Click += delegate
            {
                CallWebPage(urlService.AboutUs, "About Us");
            };

        }

        private void CallWebPage(String url, String headerText,bool isMobileOrdering = false, bool IsPrivacyPolicy = false)
        {
            UserModel.Instance.isFromMore = true;
            Intent intent = new Intent(this, typeof(WebViewActivity));
            intent.PutExtra("url", url);
            intent.PutExtra("header", headerText);
            intent.PutExtra("IsMobileOrdering", isMobileOrdering);
            intent.PutExtra("IsPrivacyPolicy", IsPrivacyPolicy);
            StartActivity(intent);
            Finish();
        }
        private void ShowHideView()
        {
            if (!Utility.GetBooleanSharedPreference("isLogin"))
            {
                myAccountViewLine.Visibility = ViewStates.Gone;
                btnMyAccount.Visibility = ViewStates.Gone;
                logoutTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                logoutTextView.Text = "LOG IN";
            }
            else
            {
                myAccountViewLine.Visibility = ViewStates.Visible;
                btnMyAccount.Visibility = ViewStates.Visible;
                logoutTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                myAccountTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                logoutTextView.Text = "LOG OUT";
            }
        }
        private void InitView(Context mContext)
        {
            btnGettingHere = FindViewById<RelativeLayout>(Resource.Id.btnGettingHere);
            btnLogout = FindViewById<RelativeLayout>(Resource.Id.btnLogout);
            btnSupportUs = FindViewById<RelativeLayout>(Resource.Id.btnSupportUs);
            btnFAQ =FindViewById<RelativeLayout>(Resource.Id.btnFAQ);
            btnShareApp = FindViewById<RelativeLayout>(Resource.Id.btnShareApp);
            btnMyAccount = FindViewById<RelativeLayout>(Resource.Id.btnMyAccount);
            myAccountViewLine = FindViewById<View>(Resource.Id.myAccountView);
            btnSeatingchart = FindViewById<RelativeLayout>(Resource.Id.btnSeatingchart);
            btnMapImageView = FindViewById<RelativeLayout>(Resource.Id.btnMapImageView);
            btnStore = FindViewById<RelativeLayout>(Resource.Id.btnStore);
            btnInbox = FindViewById<RelativeLayout>(Resource.Id.btnInbox);
            btnMobileOrdering = FindViewById<RelativeLayout>(Resource.Id.btnMobileOrdering);
            btnPrivacy = FindViewById<RelativeLayout>(Resource.Id.btnPrivacy);
            btnAboutUs = FindViewById<RelativeLayout>(Resource.Id.btnAboutUs);


            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);
            mapTextV = FindViewById<TextView>(Resource.Id.mapTextV);
            storeText = FindViewById<TextView>(Resource.Id.storeText);
            orderingText = FindViewById<TextView>(Resource.Id.orderingText);
            logoutTextView = FindViewById<TextView>(Resource.Id.logoutTextView);
            supportUsText = FindViewById<TextView>(Resource.Id.supportTextV);
            shareAppText = FindViewById<TextView>(Resource.Id.shareTextV);
            faqText = FindViewById<TextView>(Resource.Id.faqTextV);
            gettingTextV = FindViewById<TextView>(Resource.Id.gettingTextV);
            myAccountTextV = FindViewById<TextView>(Resource.Id.myAccountTextV);
            seatingchartTextV = FindViewById<TextView>(Resource.Id.SeatingTextV);
            inboxTextV = FindViewById<TextView>(Resource.Id.inboxV);
            privacyText = FindViewById<TextView>(Resource.Id.privacyTextView);
            aboutUsText = FindViewById<TextView>(Resource.Id.aboutUsTextV);

            mapTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            storeText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            orderingText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            seatingchartTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            gettingTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            shareAppText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            faqText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            logoutTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            supportUsText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            myAccountTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            inboxTextV.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            privacyText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            aboutUsText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            //logoutTextView = view.FindViewById<TextView>(Resource.Id.logoutTextView);

        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (!Utility.GetBooleanSharedPreference("isLogin"))
            {
                Intent intent = new Intent(mContext, typeof(LoginActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                Dialog dialogNoti = new Dialog(mContext);
                dialogNoti.RequestWindowFeature((int)WindowFeatures.NoTitle);
                dialogNoti.SetContentView(Resource.Layout.LoginLogoutPopup);
                dialogNoti.Window.SetLayout(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                dialogNoti.SetCancelable(true);
                dialogNoti.Show();
                TextView lblAreyouSure = (TextView)dialogNoti.FindViewById(Resource.Id.lblAreyouSure);
                TextView lblLogout = (TextView)dialogNoti.FindViewById(Resource.Id.lblLogout);
                TextView btnOkay = (TextView)dialogNoti.FindViewById(Resource.Id.btnOkay);
                TextView btnCancel = (TextView)dialogNoti.FindViewById(Resource.Id.btnCancel);

                lblAreyouSure.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                btnCancel.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                lblLogout.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
                btnOkay.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

                btnOkay.Click += delegate {
                    dialogNoti.Dismiss();
                    Utility.SetBooleanSharedPreference("isLogin", false);
                    var islogin = loginService.Logout();
                    UserModel.Instance.tickets = null;
                    UserModel.Instance.ticketsEvent = null;
                    ShowHideView();
                };
                btnCancel.Click += delegate {
                    dialogNoti.Dismiss();
                };


                //Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                //AlertDialog alert = dialog.Create();
                //alert.SetTitle("Log out");
                //alert.SetMessage("Are you sure you want to logout?");
                //alert.SetButton("Yes", (c, ev) =>
                //{
                //    Utility.SetBooleanSharedPreference("isLogin", false);
                //    var islogin = loginService.Logout();
                //    UserModel.Instance.tickets = null;
                //    UserModel.Instance.ticketsEvent = null;
                //    ShowHideView();
                //});
                //alert.SetButton2("No", (c, ev) => { });
                //alert.Show();
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
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    //Task.Delay(200);
        //    //GC.Collect(0);
        //}

		public override void OnBackPressed()
		{
            base.OnBackPressed();
            FinishAffinity();
		}
	}
}
