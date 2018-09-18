
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Android.Content;
using LAPhil.Droid;
using System;
using Android.Webkit;
using LAPhil.Urls;
using Android.Graphics;

namespace SharedLibraryAndroid
{
    [Activity(Label = "SignupActivity", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SignupActivity : AppCompatActivity
    {
        private Context mContext;
        private ImageView btnBack, btnCross;
        private WebView webView;
        public static ProgressBar progress_bar;
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup);
            mContext = this;
            var count=  FragmentManager.BackStackEntryCount;
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            btnCross = FindViewById<ImageView>(Resource.Id.btnCross);
            progress_bar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            var lblHeaderTextView = FindViewById<TextView>(Resource.Id.headerTitle);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            WebViewMethod();

            //SignupFirstPage fragment = new SignupFirstPage();
            //var transaction = FragmentManager.BeginTransaction();
            //transaction.Replace(Resource.Id.SignUp_FragmentContainer, fragment);
            //transaction.AddToBackStack("SignupFirstPage");
            //transaction.Commit();

            btnCross.Click += (sender, e) =>
            {
                Finish();
            };
            btnBack.Click += (sender, e) => 
            {
                if (FragmentManager.BackStackEntryCount > 0)
                {
                    if (FragmentManager.BackStackEntryCount==1)
                    {
                        Finish();
                    }
                    else
                    {
                        FragmentManager.PopBackStack();
                    }
                }else{
                    Finish();
                }
            };
        }
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount > 0)
            {
                if (FragmentManager.BackStackEntryCount == 1)
                {
                    base.OnBackPressed();
                    Finish();
                }
                else
                {
                    FragmentManager.PopBackStack();
                }
            }else{Finish();}
        }
        private void WebViewMethod()
        {
            webView = FindViewById<WebView>(Resource.Id.SignUp_WebView);
            webView.SetWebViewClient(new HybridWebViewClient());

            webView.LoadUrl(urlService.WebRegister);

            // Some websites will require Javascript to be enabled
            webView.Settings.JavaScriptEnabled = true;
            //          webView.LoadUrl("http://youtube.com");

            // allow zooming/panning            
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.SetSupportZoom(true);

            // scrollbar stuff            
            webView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
            // so there's no 'white line'            
            webView.ScrollbarFadingEnabled = false;

        }
        private class HybridWebViewClient : WebViewClient  
        {  
            public override bool ShouldOverrideUrlLoading(WebView view, string url)  
            {  
  
                view.LoadUrl(url);  
                return true;  
            }  
            public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)  
            {  
                base.OnPageStarted(view, url, favicon);
                progress_bar.Visibility = ViewStates.Visible;
            }  
            public override void OnPageFinished(WebView view, string url)  
            {  
                base.OnPageFinished(view, url);  

                progress_bar.Visibility = ViewStates.Gone;
            }
            public override void OnReceivedError(WebView view, ClientError errorCode, string description, string failingUrl)
            {
                base.OnReceivedError(view, errorCode, description, failingUrl);
            }
        } 


    }
}