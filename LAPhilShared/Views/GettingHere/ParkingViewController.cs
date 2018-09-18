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
    public partial class ParkingViewController : UITableViewController
    {
        List<string> itemData;
        public ParkingViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.Title = "Parking";
            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
            UIImage.FromFile("Others/btnBack.png"), UIBarButtonItemStyle.Plain, (sender, args) => {
                this.NavigationController.PopViewController(true);
            }), true);
            this.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };


            itemData = LAPhilShared.SharedClass.GetParkingData();
            tableListView.RegisterNibForCellReuse(UINib.FromName("WhenYouTableViewCell", null), "WhenYouTableViewCell");
            tableListView.Source = new DataSource(this, itemData);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);
            if (index == 0)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var nearbyDiningViewController = storyboard.InstantiateViewController("NearbyDiningViewController") as NearbyDiningViewController;
                NavigationController.PushViewController(nearbyDiningViewController, true);

            }
            else if (index == 1)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var chartViewController = storyboard.InstantiateViewController("ChartViewController") as ChartViewController;
                NavigationController.PushViewController(chartViewController, true);

            }
            return null;

        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        class DataSource : UITableViewSource
        {
            static readonly NSString CellIdentifierWhen = new NSString("WhenYouTableViewCell");

            nfloat hightDisctiontion = 100;


            private WhenYouHereViewController _WhenYouHereViewController;
            List<string> tabledata;

            public DataSource(UIViewController parentViewController, List<string> items)
            {
                _WhenYouHereViewController = parentViewController as WhenYouHereViewController;
                tabledata = items;
            }

            // Customize the number of sections in the table view.
            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return tabledata.Count + 1;
            }
            // Customize the appearance of table view cells.
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                WhenYouTableViewCell cell = tableView.DequeueReusableCell(CellIdentifierWhen, indexPath) as WhenYouTableViewCell;
                return cell;

            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);
                tableView.DeselectRow(indexPath, true);
                _WhenYouHereViewController.ViewControllerAtIndex(indexPath.Row);

            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                    return 400;
            }

        }

    }
}