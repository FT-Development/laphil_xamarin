using Foundation;
using System;
using UIKit;

namespace LAPhil.iOS
{
    public partial class MyPageViewController : UIPageViewController
    {
        public MyPageViewController (IntPtr handle) : base (handle)
        {
        }
        //public MyPageViewController() : base("MyPageViewController", null)
        //{
        //}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}