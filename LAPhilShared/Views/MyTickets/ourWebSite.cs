using Foundation;
using System;
using UIKit;
using LAPhil.Platform;
using LAPhil.Application;
using LAPhil.Urls;

namespace LAPhil.iOS
{
    public partial class ourWebSite : UIViewController
    {
        LAPhilUrlService urlService = ServiceContainer.Resolve<LAPhilUrlService>();

        public ourWebSite (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            btnClose.TouchUpInside += CloseView;
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var urlsRegister = urlService.WebTickets;

            this.webViewSignUp.ScalesPageToFit = true;
            this.webViewSignUp.Delegate = new MyCustomWebViewDelegate();
            this.webViewSignUp.LoadRequest(new NSUrlRequest(new NSUrl(urlsRegister)));

        }

        private void CloseView(object sender, EventArgs e)
        {
            Console.WriteLine("Pop CloseView ");
            this.DismissViewController(true, null);
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
                Console.WriteLine("LoadingFinished");
            }


            public override void LoadStarted(UIWebView webView)
            {
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