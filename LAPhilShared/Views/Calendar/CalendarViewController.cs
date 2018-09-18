using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using LAPhil.Platform;


namespace LAPhil.iOS
{
    public partial class CalendarViewController : UIViewController
    {
        public CalendarViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.DarkGray;

            this.NavigationItem.Title = "Choose Your Date(s)";
            var customRightButton = new UIBarButtonItem();
            var customLeftButton = new UIBarButtonItem(
                UIImage.FromFile("Others/backConnect.png"),
            UIBarButtonItemStyle.Plain,
            (s, e) => {
                System.Diagnostics.Debug.WriteLine("Left button tapped");
                this.NavigationController.PopViewController(true);
            });
            customLeftButton.TintColor = UIColor.White;
            this.NavigationItem.LeftBarButtonItem = customLeftButton;
            this.NavigationItem.RightBarButtonItem = customRightButton;

        }
    }
}