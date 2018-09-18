using Foundation;
using LAPhil.Urls;
using System;
using UIKit;

namespace LAPhil.iOS
{
    public partial class WebForgotPasswordViewController : UIViewController
    {
        public static UIActivityIndicatorView activity = new UIActivityIndicatorView();

        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();

        public WebForgotPasswordViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.TouchUpInside += CloseView;
            btnBack.TouchUpInside += BackView;


            activity = this.activityIndicator;
            this.NavigationItem.Title = "Forgotten Password";
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

        private void CloseView(object sender, EventArgs e)
        {
            Console.WriteLine("Pop CloseView ");
            this.DismissViewController(true, null);
        }
        private void BackView(object sender, EventArgs e)
        {
            Console.WriteLine("Pop BackView ");
            this.NavigationController.PopViewController(true);
            //this.DismissViewController(true, null);
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var urlsFoodWine = urlService.WebForgotPassword;

            Console.WriteLine("urlsFoodWine : {0}", urlsFoodWine);

            this.webViewSignUp.ScalesPageToFit = true;
            this.webViewSignUp.Delegate = new MyCustomWebViewDelegate();
            this.webViewSignUp.LoadRequest(new NSUrlRequest(new NSUrl(urlsFoodWine)));

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