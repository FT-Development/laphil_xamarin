using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
    public class WhenYouHereWebViews : Fragment
    {
        private TextView lblHeaderTextView;
        private Context mContext;
        private ImageView btnBack;
        private WebView webView;
        public static ProgressBar progress_bar;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.WhenYouHereWebViews, container, false);
            mContext = this.Activity;

            InitView(v, mContext);

            lblHeaderTextView.Text = Arguments.GetString("header");
            btnBack.Click += delegate
            {
                FragmentManager.PopBackStack();
            };

            Task.Delay(200);
            WebViewMethod();
            return v.RootView;
        }
        public void InitView(View v, Context mContext)
        {
            btnBack = v.FindViewById<ImageView>(Resource.Id.btnBack);
            lblHeaderTextView = v.FindViewById<TextView>(Resource.Id.lblHeaderTextView);

            progress_bar = v.FindViewById<ProgressBar>(Resource.Id.progress_bar);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
             webView = v.FindViewById<WebView>(Resource.Id.webView);
        }

        private void WebViewMethod()
        {
            
            webView.SetWebViewClient(new HybridWebViewClient());
            //webView.SetWebViewClient(new WebViewClient());
            webView.LoadUrl(Arguments.GetString("url"));

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
