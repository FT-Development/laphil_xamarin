#if __IOS__
using System;
using UIKit;
using CoreGraphics;
namespace LAPhil.Platform
{
    public static class UIColorExtensions
    {
        public static UIImage Image(this UIColor source, CGRect rect)
        {
            UIGraphics.BeginImageContext(rect.Size);

            var context = UIGraphics.GetCurrentContext();
            context.SetFillColor(source.CGColor);
            context.FillRect(rect);

            var image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return image;
        }
    }
}
#endif