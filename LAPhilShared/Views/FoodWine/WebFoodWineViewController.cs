using Foundation;
using System;
using UIKit;
using LAPhil;
using LAPhil.Urls;
using LAPhil.Platform;

namespace LAPhil.iOS
{
    public partial class WebFoodWineViewController : UIViewController
    {
        [Outlet]
        public UIActivityIndicatorView activityIndicator { get; set; }

        [Outlet]
        public UIWebView webViewSignup { get; set; }

        public static UIActivityIndicatorView activity = new UIActivityIndicatorView();

        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();

        public UIBarButtonItem button;
        NSObject observer;

        public WebFoodWineViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            activity = this.activityIndicator;
            NavigationItem.Title = "Food + Wine";

            NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };

            if (observer != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
                observer = null;
            }
            observer = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"Notification_WebView", notification_LoadData);

        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var urlsFoodWine = urlService.WebFoodWine;
            Console.WriteLine("urlsFoodWine : {0}", urlsFoodWine);

            this.webViewSignup.ScalesPageToFit = true;
            this.webViewSignup.Delegate = new MyCustomWebViewDelegate();
            this.webViewSignup.LoadRequest(new NSUrlRequest(new NSUrl(urlsFoodWine)));

            mYConfigureDefaultBackButtonWebView(this.webViewSignup);



        }

        void notification_LoadData(NSNotification obj)
        {
            Console.WriteLine(" CALL notification_LoadData");

            if (this.webViewSignup.CanGoBack)
            {
                Console.WriteLine("BACK Button ");
                this.NavigationItem.SetLeftBarButtonItem(button, animated: true);
                this.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;
            }
            else
            {
                Console.WriteLine("False ");
                this.NavigationItem.SetLeftBarButtonItem(null, animated: true);
            }
        }

        public void mYConfigureDefaultBackButtonWebView(UIWebView tmpWebView)
        {
            button = new UIBarButtonItem(
                image: UIImage.FromFile("Others/btnBackChevron.png"),
                style: UIBarButtonItemStyle.Plain,
                handler: OnDefaultBackButton
            );


            if (tmpWebView.CanGoBack)
            {
                Console.WriteLine("BACK Button ");
                this.NavigationItem.SetLeftBarButtonItem(button, animated: true);
                this.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;
            }
            else
            {
                Console.WriteLine("False ");
                this.NavigationItem.SetLeftBarButtonItem(null, animated: true);
            }

            if (this.NavigationController != null && this.NavigationController.NavigationBar != null)
            {
                this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
                {
                    ForegroundColor = UIColor.White
                };
            }
        }

        private void OnDefaultBackButton(object sender, EventArgs e)
        {
            if ((this.webViewSignup != null) && this.webViewSignup.CanGoBack)
            {
                Console.WriteLine("BACK Button Web");
                this.webViewSignup.GoBack();
            }
            else
            {
                this.NavigationController.PopViewController(animated: true);
            }

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

                NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"Notification_WebView", null);

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
                webView.EvaluateJavascript(JavaScriptForRemoveHeader);//TODO U

                var JavaScriptForRemoveGrayColor = "javascript:(function(){"
                    + "document.getElementsByClassName('hero-background phatvideo-bg videobg-id-0')[0].style.height = '60%';"
                    + "})()";

                webView.EvaluateJavascript(JavaScriptForRemoveGrayColor);


            }

        }

    }
}