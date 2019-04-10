using System;

using UIKit;
using UrbanAirship;

using LAPhil.Platform;

namespace LAPhilShared.Views.UrbanAirship
{
    public partial class MessageCenterViewController : UAMessageCenterSplitViewController
    {
        void HandleEventHandler(object sender, EventArgs e)
        {
        }

        public MessageCenterViewController() : base()
        {
        }

        public MessageCenterViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var style = new UAMessageCenterStyle();
            // Customize the style object
            style.NavigationBarColor = UIColor.Black;
            style.TitleColor = UIColor.White;
            style.TintColor = UIColor.White;
            this.Style = style;

            AddBackButton(true);

            ListViewController.View.LayoutIfNeeded();
        }

        private void AddBackButton(Boolean inNavbar) 
        {
         
            if (inNavbar)
            {
                var button = new UIBarButtonItem(
                    image: UIImage.FromFile("Others/btnBackChevron.png"), 
                    style: UIBarButtonItemStyle.Plain,
                    handler: OnBackClick);

                ListViewController.NavigationItem.SetLeftBarButtonItem(button, animated: true);
                ListViewController.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;
                if (ListViewController.NavigationController != null && ListViewController.NavigationController.NavigationBar != null)
                {
                    ListViewController.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
                    {
                        ForegroundColor = UIColor.White
                    };
                }
            }else 
            {

                UIButton button = new UIButton();
                button.BackgroundColor = UIColor.Black;
                button.TintColor = UIColor.White;
                button.SetTitle("Close", UIControlState.Normal);
                button.SetTitle("Close", UIControlState.Highlighted);
                button.AddTarget(OnBackClick, UIControlEvent.TouchUpInside);

                ListViewController.View.AddSubview(button);
                button.TranslatesAutoresizingMaskIntoConstraints = false;

                button.CenterXAnchor.ConstraintEqualTo(ListViewController.View.CenterXAnchor).Active = true;
                button.WidthAnchor.ConstraintEqualTo(ListViewController.View.Frame.Width).Active = true;
                button.HeightAnchor.ConstraintEqualTo(60).Active = true;
                var contraint = button.BottomAnchor.ConstraintEqualTo(ListViewController.View.BottomAnchor);
                contraint.Constant = -28;
                contraint.Active = true;

            }
        }

        public void DisplayMessage(UAInboxMessage message)
        {
            ListViewController.DisplayMessage(message.Description);
        }

        void OnBackClick(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

    }
}
