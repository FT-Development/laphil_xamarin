using UIKit;
using Foundation;
using System;
using LAPhil.Application;
using LAPhil.Urls;
using LAPhil.Platform;

namespace LAPhil.iOS
{
    [Register("GenericWebViewController")]
    public class GenericWebViewController : UIViewController
    {
        public string url = "";
        public string pageTitle = "";
        public bool removeHeader = true;
        public bool removePrivacyAlert = false;

        [Outlet]
        public UIActivityIndicatorView activityIndicator { get; set; }

        [Outlet]
        public UIWebView webView { get; set; }

        public GenericWebViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.Title = pageTitle;
            this.ConfigureDefaultBackButtonWebView(this.webView);
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Console.WriteLine("urlsAccessibility : {0}", url);

            this.webView.ScalesPageToFit = true;
            this.webView.Delegate = new CustomWebViewDelegate(activityIndicator, removeHeader, removePrivacyAlert);
            this.webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));
        }

    }

    public class CustomWebViewDelegate : UIWebViewDelegate
    {
        private UIActivityIndicatorView activityIndicatorView;
        private bool removeHeader, removePrivacyAlert;

        public CustomWebViewDelegate(UIActivityIndicatorView activityIndicatorView, bool removeHeader, bool removePrivacyAlert) {
            this.activityIndicatorView = activityIndicatorView;
            this.removeHeader = removeHeader;
            this.removePrivacyAlert = removePrivacyAlert;
        }

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
            activityIndicatorView.StopAnimating();
            activityIndicatorView.Hidden = true;
            Console.WriteLine("LoadingFinished");
        }


        public override void LoadStarted(UIWebView webView)
        {
            activityIndicatorView.StartAnimating();
            activityIndicatorView.Hidden = false;
            Console.WriteLine("LoadStarted");
            //Harish_A3

            if (removeHeader)
            {
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

            if (removePrivacyAlert) {

                var JavaScriptForRemoveAlert = "javascript:(function() {" 
                    + "var privacyAlert = document.getElementsByClassName('alert privacy-popup alert-dismissible')[0]; "
                    + "privacyAlert.parentNode.removeChild(privacyAlert);"
                    + "})()";
                webView.EvaluateJavascript(JavaScriptForRemoveAlert);

            }

        }

    }

}
