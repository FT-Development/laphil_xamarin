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

namespace HollywoodBowl.Droid
{
       public class FoodAndWineView : Fragment
    {
        private Context mContext;
        private WebView webView;
        public static ProgressBar progress_bar;
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.FoodAndWineView, container, false);
            mContext = this.Activity;

            InitView(v, mContext);
        progress_bar.Visibility = ViewStates.Visible;
        Activity.RunOnUiThread(() => {
                        
                WebViewMethod();

            });
            return v.RootView;
        }
        public void InitView(View v, Context mContext)
        {
            progress_bar = v.FindViewById<ProgressBar>(Resource.Id.progress_bar);
            webView = v.FindViewById<WebView>(Resource.Id.webView);
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
            public override void OnReceivedError(WebView view, ClientError errorCode, string description, string failingUrl)
            {
                base.OnReceivedError(view, errorCode, description, failingUrl);

                progress_bar.Visibility = ViewStates.Gone;
            }
        }
    }
}