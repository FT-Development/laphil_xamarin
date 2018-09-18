#if __IOS__
using System;
using UIKit;
using CoreGraphics;
using Foundation;


namespace LAPhil.Platform
{
    public static class UITabBarExtrensions
    {
        public static void SelectionBackgroundColor(this UITabBar tabBar, UIColor color)
        {
            var tabWidth = tabBar.Frame.Size.Width / (nfloat) tabBar.Items.Length;
            var size = new CGSize(width: tabWidth, height: tabBar.Frame.Height);

            var rect = new CGRect(x: 0, y: 0, width: size.Width, height: size.Height);

            UIGraphics.BeginImageContext(rect.Size);

            var context = UIGraphics.GetCurrentContext();
            context.SetFillColor(color.CGColor);
            context.FillRect(rect);

            var image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            //var resizableImage = image.CreateResizableImage(UIEdgeInsets.Zero);
            tabBar.SelectionIndicatorImage = image;
        }
    }
}
#endif
