using System.Threading.Tasks;
using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using LAPhil.Urls;
using Android.Content.PM;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FoodWineActivity : Activity
    {
        private Context mContext;
        private WebView webView;
        public static ProgressBar progress_bar;
        public static ImageView backBtn;
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FoodAndWineView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;
            InitView(mContext);

            Task.Delay(200);
            WebViewMethod();

            backBtn.Click += delegate {
                if (webView.CanGoBack())
                {
                    webView.GoBack();
                }
            };
        }
        public void InitView(Context mContext)
        {
            progress_bar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            backBtn = FindViewById<ImageView>(Resource.Id.backBtn);
            webView = FindViewById<WebView>(Resource.Id.webView);
            var tabBar = FindViewById<LinearLayout>(Resource.Id.lytCustomBottom);
            tabBar.AddView(new TabBarView(mContext));
        }

        private void WebViewMethod()
        {
            webView.SetWebViewClient(new HybridWebViewClient());
            webView.LoadUrl(urlService.WebFoodWine);
            // Some websites will require Javascript to be enabled
            webView.Settings.JavaScriptEnabled = true;
            // allow zooming/panning            
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.SetSupportZoom(true);

            // scrollbar stuff            
            webView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
            // so there's no 'white line'            
            webView.ScrollbarFadingEnabled = false;
            WebSettings settings = webView.Settings;
            settings.DomStorageEnabled = true;

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
                view.LoadUrl("javascript:(function(){"
                    + "document.getElementsByClassName('hero-background phatvideo-bg videobg-id-0')[0].style.height = '60%';"
                    + "})()");
                view.LoadUrl("javascript:(function() { " +
                        "var head = document.getElementsByTagName('header')[0];"
                        + "head.parentNode.removeChild(head);" +
                        "})()");
                if (view.CanGoBack())
                {
                    backBtn.Visibility=ViewStates.Visible;
                }
                else
                {
                    backBtn.Visibility = ViewStates.Gone;
                }
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
            FinishAffinity();
        }
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
