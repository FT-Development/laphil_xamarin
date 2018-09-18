using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Webkit;
using System.Threading.Tasks;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class WebViewActivity : Activity
    {  
        private TextView lblHeaderTextView;
        private Context mContext;
        private ImageView btnBack;
        private WebView webView;
        private TabBarView tabBarView;
        public static ProgressBar progress_bar;
        public String Url { get; set; }
        public String HeaderTitle { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WhenYouHereWebViews);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;
            InitView(mContext);

            Url= Intent.GetStringExtra("url");
            HeaderTitle = Intent.GetStringExtra("header");

            lblHeaderTextView.Text = HeaderTitle;
            btnBack.Click += delegate
            {
                if (webView.CanGoBack())
                {
                    webView.GoBack();
                }
                else
                {
                    if (UserModel.Instance.isFromMore == true)
                    {
                        Intent intent = new Intent(this, typeof(MoreActivity));
                        StartActivity(intent);
                        Finish();
                    }
                    else if (UserModel.Instance.isFromConcert == true)
                    {
                        Finish();
                        OverridePendingTransition(Resource.Animation.Slide_in_left, Resource.Animation.Slide_out_right);
                    }
                    else
                    {
                        Intent intent = new Intent(this, typeof(SupportUsActivity));
                        StartActivity(intent);
                        Finish();
                    }
                }
            };

            Task.Delay(200);
            WebViewMethod();
        }
        public void InitView(Context mContext)
        {
            btnBack =FindViewById<ImageView>(Resource.Id.btnBack);
            lblHeaderTextView = FindViewById<TextView>(Resource.Id.lblHeaderTextView);

            var lytCustomBottom = FindViewById<LinearLayout>(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);

            progress_bar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
             webView = FindViewById<WebView>(Resource.Id.webView);
        }

        private void WebViewMethod()
        {
            

            webView.SetWebViewClient(new WebViewClient());
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
            webView.SetWebViewClient(new HybridWebViewClient());
            WebSettings settings = webView.Settings;
            settings.DomStorageEnabled = true;
            //webView.SetWebChromeClient();
        }

        private class HybridWebViewClient : WebViewClient  
        {  
            public override bool ShouldOverrideUrlLoading(WebView view, string url)  
            {  
  
                view.LoadUrl(url);  
                return true;  
            }  
            public override void OnPageStarted(WebView view, string url, Bitmap favicon)  
            {  
                base.OnPageStarted(view, url, favicon);
                progress_bar.Visibility = ViewStates.Visible;
            }  
            public override void OnPageFinished(WebView view, string url)  
            {  
                base.OnPageFinished(view, url);  

                progress_bar.Visibility = ViewStates.Gone;
            }
            public override void OnLoadResource(WebView view, string url)
            {
                base.OnLoadResource(view, url);
                var tempWeb = "laphil";
                if (tempWeb.Contains(url))
                {
                    view.LoadUrl("javascript:(function(){"
                       + "document.getElementsByClassName('hero-background phatvideo-bg videobg-id-0')[0].style.height = '60%';"
                       + "})()");
                    view.LoadUrl("javascript:(function() { " +
                            "var head = document.getElementsByTagName('header')[0];"
                            + "head.parentNode.removeChild(head);" +
                            "})()");
                }
                progress_bar.Visibility = ViewStates.Gone;
            }
			public override void OnReceivedError(WebView view, ClientError errorCode, string description, string failingUrl)
            {
                base.OnReceivedError(view, errorCode, description, failingUrl);
                progress_bar.Visibility = ViewStates.Gone;
            }
        } 
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            if (UserModel.Instance.isFromMore == true)
            {
                Intent intent = new Intent(this, typeof(MoreActivity));
                StartActivity(intent);
                Finish();
            }
            else if (UserModel.Instance.isFromConcert == true)
            {
                Finish();
                OverridePendingTransition(Resource.Animation.Slide_in_left, Resource.Animation.Slide_out_right);
            }
            else
            {
                Intent intent = new Intent(this, typeof(SupportUsActivity));
                StartActivity(intent);
                Finish();
            }
        }
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    Task.Delay(400);
        //    GC.Collect(0);
        //}
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && webView.CanGoBack())
            {
                webView.GoBack();

                return true;
            }

            return base.OnKeyDown(keyCode, e);
        }
    }
}
