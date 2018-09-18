using System;
using System.Reactive;
using System.Reactive.Linq;
using Foundation;
using UIKit;
using System.Collections.Generic;
using LAPhilShared;
using LAPhil.iOS.TableViewCell;


namespace LAPhil.iOS
{
    public partial class ChartViewController : UIViewController
    {
        public ChartViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.Title = "Setting Chart";

            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
            UIImage.FromFile("Others/btnBack.png"), UIBarButtonItemStyle.Plain, (sender, args) => {
                this.NavigationController.PopViewController(true);
            }), true);
            this.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

    }
}