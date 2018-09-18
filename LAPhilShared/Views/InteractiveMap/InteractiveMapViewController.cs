using CoreGraphics;
using Foundation;
using LAPhil.Application;
using LAPhil.Platform;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LAPhil.iOS
{
    public partial class InteractiveMapViewController : UIViewController, IUIScrollViewDelegate, IDisposable
    {
        //public InteractiveMapViewController() : base("InteractiveMapViewController", null)
        //{
        //}
        public InteractiveMapViewController(IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationItem.Title = "Map";
            this.ConfigureDefaultBackButton();
            imageView.ContentMode = UIViewContentMode.Center;
            imageView.Frame = CGRect.FromLTRB(0, 0,imageView.Image.Size.Width, imageView.Image.Size.Height);
            scrollView.ContentSize = imageView.Image.Size;
            scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return imageView; };
            scrollView.DidZoom += (sender, e) => { updateConstraintsForSize(View.Bounds.Size); };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            updateMinZoomScaleForSize(View.Bounds.Size);
        }

        //public UIView ViewForZooming(UIScrollView scrollView)
        //{
        //    return imageView;
        //}

        public void updateMinZoomScaleForSize(CGSize size)
        {
            var widthScale = size.Width / imageView.Bounds.Width;
            var heightScale = size.Height / imageView.Bounds.Height;
            var minScale = (nfloat) Math.Min(widthScale, heightScale);
            if (minScale > 1) {
                minScale = 1;
            }
            scrollView.MinimumZoomScale = minScale;
            // scrollView.MaximumZoomScale = 2;
            scrollView.MaximumZoomScale = 1;
            scrollView.ZoomScale = minScale;
            Console.WriteLine("scrollView.ZoomScale {0} {1}", scrollView.ZoomScale, minScale);
        }

        //public void ScrollViewDidZoom(UIScrollView scrollView)
        //{
        //    updateConstraintsForSize(View.Bounds.Size);
        //}

        public void updateConstraintsForSize(CGSize size)
        {
            var yOffset = (nfloat) Math.Max(0, (size.Height - imageView.Frame.Height) / 2);
            imageViewTopConstraint.Constant = yOffset;
            imageViewBottomConstraint.Constant = yOffset;

            var xOffset = (nfloat)Math.Max(0, (size.Width - imageView.Frame.Width) / 2);

            imageViewLeadingConstraint.Constant = xOffset;
            imageViewTrailingConstraint.Constant = xOffset;

            View.LayoutIfNeeded();
         }
    }
}

