using System;

using UIKit;

namespace LAPhil.iOS
{
    public partial class ContentViewController : UIViewController
    {
        //public ContentViewController() : base("ContentViewController", null)
        //{
        //}

        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();
        //    // Perform any additional setup after loading the view, typically from a nib.
        //}

        //public override void DidReceiveMemoryWarning()
        //{
        //    base.DidReceiveMemoryWarning();
        //    // Release any cached data, images, etc that aren't in use.
        //}

        public int pageIndex = 0;
        public string titleText;
        public string imageFile;
        public string imageLogo;

        public ContentViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            imageView.Image = UIImage.FromBundle(imageFile);
            OnBoardSlideIcon.Image = UIImage.FromBundle(imageLogo);
            lblScreenTitle.Text = titleText;
        }
    }
}

