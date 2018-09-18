#if __IOS__
using System;
using UIKit;


namespace LAPhil.Platform
{
    public static class LAPhilUIViewControllerExtensions
    {
        public static UIWebView webView;
        public static void ConfigureDefaultBackButtonWebView(this UIViewController source, UIWebView tmpWebView)
        {
            webView = tmpWebView;

            var button = new UIBarButtonItem(
                image: UIImage.FromFile("Others/btnBackChevron.png"),
                style: UIBarButtonItemStyle.Plain,
                handler: source.OnDefaultBackButton
            );



            source.NavigationItem.SetLeftBarButtonItem(button, animated: true);
            source.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;
            if (source.NavigationController != null && source.NavigationController.NavigationBar != null)
            {
                source.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
                {
                    ForegroundColor = UIColor.White
                };
            }
        }

        public static void ConfigureDefaultBackButton(this UIViewController source)
        {

            var button = new UIBarButtonItem(
                image: UIImage.FromFile("Others/btnBackChevron.png"),
                style: UIBarButtonItemStyle.Plain,
                handler: source.OnDefaultBackButton
            );



            source.NavigationItem.SetLeftBarButtonItem(button, animated: true);
            source.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;
            if (source.NavigationController != null && source.NavigationController.NavigationBar != null)
            {
                source.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
                {
                    ForegroundColor = UIColor.White
                };    
            }
        }

        public static void OnDefaultBackButton(this UIViewController source, object sender, EventArgs args)
        {
            if ((webView != null) && webView.CanGoBack)
            {
                Console.WriteLine("BACK Button Web");
                webView.GoBack();
            }
            else
            {
                source.NavigationController.PopViewController(animated: true);
            }

        }
    }
}
#endif