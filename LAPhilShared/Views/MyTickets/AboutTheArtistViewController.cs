using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using LAPhilShared;

namespace LAPhil.iOS
{
    public partial class AboutTheArtistViewController : UITableViewController
    {
        public AboutTheArtistViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationItem.Title = "About the Artist";
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };

            var customLeftButton = new UIBarButtonItem(
            UIImage.FromFile("Others/backConnect.png"),
            UIBarButtonItemStyle.Plain,
            (s, e) => {
                System.Diagnostics.Debug.WriteLine("Left button tapped");
                this.NavigationController.PopViewController(true);
            }
            );
            customLeftButton.TintColor = UIColor.White;
            this.NavigationItem.LeftBarButtonItem = customLeftButton;

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }
    }
}