using System;

using UIKit;
using UrbanAirship;

using LAPhil.Platform;

namespace LAPhilShared.Views.UrbanAirship
{
    public partial class MessageCenterViewController : UAMessageCenterSplitViewController
    {
        public UIButton button;

        public MessageCenterViewController() : base()
        {
        }

        public MessageCenterViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.ConfigureDefaultBackButton();
            var style = new UAMessageCenterStyle();
            // Customize the style object
            style.NavigationBarColor = UIColor.Black;
            style.TitleColor = UIColor.White;
            style.TintColor = UIColor.White;
            //style.TitleFont = UIFont
            //style.titleFont = UIFont(name: "Roboto-Regular", size: 17.0)
            //style.cellTitleFont = UIFont(name: "Roboto-Bold", size: 14.0)
            //style.cellDateFont = UIFont(name: "Roboto-Light", size: 12.0)
            //this.Style = UAirship.DefaultMessageCenter.Style;
            this.Style = style;

            button = new UIButton();
            button.BackgroundColor = UIColor.Black;
            button.TintColor = UIColor.White;
            button.SetTitle("Close", UIControlState.Normal);
            button.SetTitle("Close", UIControlState.Highlighted);
            button.AddTarget((sender, e) => DismissViewController(true, null), UIControlEvent.TouchUpInside);
            // button.AddTarget((object sender, EventArgs ea) => PopViewController(animated: true), UIControlEvent.TouchUpInside);
            ListViewController.View.AddSubview(button);
            button.TranslatesAutoresizingMaskIntoConstraints = false;
            button.CenterXAnchor.ConstraintEqualTo(ListViewController.View.CenterXAnchor).Active = true;
            button.WidthAnchor.ConstraintEqualTo(ListViewController.View.Frame.Width).Active = true;
            button.HeightAnchor.ConstraintEqualTo(60).Active = true;
            var contraint = button.BottomAnchor.ConstraintEqualTo(ListViewController.View.BottomAnchor);
            contraint.Constant = -28;
            contraint.Active = true;

            ListViewController.View.LayoutIfNeeded();
        }

        public void DisplayMessage(UAInboxMessage message)
        {
            ListViewController.DisplayMessage(message.Description);
        }
    }
}
