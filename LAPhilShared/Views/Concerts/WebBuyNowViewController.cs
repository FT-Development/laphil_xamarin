using Foundation;
using System;
using UIKit;

namespace LAPhil.iOS
{
    public partial class WebBuyNowViewController : UIViewController
    {
        public string strTitle;
        public string strBuyURLs;
        public static UIActivityIndicatorView activity = new UIActivityIndicatorView();

        public WebBuyNowViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            activity = this.activityIndicator;

            this.NavigationItem.Title = strTitle;
            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
            UIImage.FromFile("Others/btnBack.png"), UIBarButtonItemStyle.Plain, (sender, args) => {
                this.NavigationController.PopViewController(true);
            }), true);
            this.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };

        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //var urlsRegister = "https://lapatester:p@ssw0rd@tickets-dev.laphil.com/account/register";
            var urlsChart = this.strBuyURLs;
            urlsChart = urlsChart.Replace("https://", "https://lapatester:p@ssw0rd@");
            Console.WriteLine("urlsChart : {0}", urlsChart);

            this.WebViewSignup.ScalesPageToFit = true;
            this.WebViewSignup.Delegate = new MyCustomWebViewDelegate();
            this.WebViewSignup.LoadRequest(new NSUrlRequest(new NSUrl(urlsChart)));
           }


        public class MyCustomWebViewDelegate : UIWebViewDelegate
        {
            // get's never called:
            public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
            {
                Console.WriteLine("ShouldStartLoad");
                return true;
            }

            public override void LoadFailed(UIWebView webView, NSError error)
            {
                Console.WriteLine("LoadFailed");
            }


            public override void LoadingFinished(UIWebView webView)
            {
                activity.StopAnimating();
                activity.Hidden = true;
                Console.WriteLine("LoadingFinished");
            }


            public override void LoadStarted(UIWebView webView)
            {
                activity.StartAnimating();
                activity.Hidden = false;
                Console.WriteLine("LoadStarted");
                //Harish_A3
                var JavaScriptForRemoveHeader = "javascript:(function() { " +
                "var head = document.getElementsByTagName('header')[0];"
                + "head.parentNode.removeChild(head);" +
                    "})()";
                webView.EvaluateJavascript(JavaScriptForRemoveHeader);

                var JavaScriptForRemoveGrayColor = "javascript:(function(){"
                    + "document.getElementsByClassName('hero-background phatvideo-bg videobg-id-0')[0].style.height = '60%';"
                    + "})()";

                webView.EvaluateJavascript(JavaScriptForRemoveGrayColor);
            }

        }
    }
}