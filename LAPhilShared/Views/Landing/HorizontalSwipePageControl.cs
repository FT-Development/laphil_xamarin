using System;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace HorizontalSwipe
{
    public class HorizontalSwipePageControl : UIPageControl
    {

        public HorizontalSwipePageControl()
        {
            ActiveImagePurple = UIImage.FromBundle("OnBoardSlide/white-pip-circle.png");
            InactiveImageWhite = UIImage.FromBundle("OnBoardSlide/white-pip-full.png");

        }


        public UIImage ActiveImagePurple { get; set; }
        public UIImage InactiveImageWhite { get; set; }

        private void UpdateDots()
        {
            
            var myX = -50;
            for (int index = 0; index < Subviews.Length; index++)
            {
                myX = myX + 20;
                var view = Subviews[index];
                var dot = view.Subviews.OfType<UIImageView>().Select(subview => subview).FirstOrDefault();

                if (dot == null)
                {
                    dot = new UIImageView(new CGRect(myX, 0, view.Frame.Width + 20, view.Frame.Height + 20));
                    view.AddSubview(dot);
                }

                dot.ContentMode = UIViewContentMode.Center;

                dot.Image = index == CurrentPage
                    ? ActiveImagePurple
                    : InactiveImageWhite;
            }
        }

        public override nint CurrentPage
        {
            get { return base.CurrentPage; }
            set { base.CurrentPage = value; UpdateDots(); } 
        }
    }
}