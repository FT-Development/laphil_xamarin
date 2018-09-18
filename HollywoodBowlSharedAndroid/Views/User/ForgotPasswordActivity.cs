using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "ForgotActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ForgotPasswordActivity : AppCompatActivity
    {
        private TextView lblHeaderTextView;
        private Context mContext;
        private ImageView btnBack;
        private WebView webView;
        private String Url;

        public static ProgressBar progress_bar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ForgotPasswordLayout);
            mContext = this;
            InitView(mContext);

            Url = Intent.GetStringExtra("url");
            btnBack.Click += delegate
            {
                Finish();
            };

            WebViewMethod();
        }

        public void InitView(Context mContext)
        {
            btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            lblHeaderTextView = FindViewById<TextView>(Resource.Id.lblHeaderTextView);

            progress_bar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            webView = FindViewById<WebView>(Resource.Id.webView);
        }

        private void WebViewMethod()
        {

            webView.SetWebViewClient(new HybridWebViewClient());
            //webView.SetWebViewClient(new WebViewClient());
            webView.LoadUrl(Url);

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

            //webView.SetWebChromeClient();
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
