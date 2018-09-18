using Foundation;
using System;
using UIKit;
namespace LAPhilShared.Views
{
    [Register("WebViewController")]
    public class WebViewController: UIViewController
    {
        public String url = "";

        public UIWebView webView = new UIWebView();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // webView.delegate = this;
            webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));
            webView.ScalesPageToFit = true;
            View.ContentMode = UIViewContentMode.ScaleToFill;
            webView.Frame = View.Frame;
            View.AddSubview(webView);
        }
    }
}
