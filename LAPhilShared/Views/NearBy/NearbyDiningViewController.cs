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
    public partial class NearbyDiningViewController : UIViewController
    {
        List<string> itemData;
        public NearbyDiningViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.Title = "Nearby Dining";

            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
            UIImage.FromFile("Others/btnBack.png"), UIBarButtonItemStyle.Plain, (sender, args) => {
                this.NavigationController.PopViewController(true);
            }), true);
            this.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };

            itemData = LAPhilShared.SharedClass.GetWhereYourHereData();
            this.tableNearby.RegisterNibForCellReuse(UINib.FromName("NearbyTableViewCell", null), "NearbyTableViewCell");
            this.tableNearby.Source = new DataSource(this, itemData);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);
            return null;

        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        class DataSource : UITableViewSource
        {
            static readonly NSString CellIdentifier = new NSString("NearbyTableViewCell");

            nfloat hightDisctiontion = 100;

            private MoreTableViewController _moreTableViewController;
            List<string> tabledata;

            public DataSource(UIViewController parentViewController, List<string> items)
            {
                _moreTableViewController = parentViewController as MoreTableViewController;
                tabledata = items;
            }

            // Customize the number of sections in the table view.
            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return tabledata.Count;
            }
            // Customize the appearance of table view cells.
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                    NearbyTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as NearbyTableViewCell;
                    return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);
                tableView.DeselectRow(indexPath, true);
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Calcumation hight : {0}",hightDisctiontion);
                return 600; //hightDisctiontion + 100;
            }

        }
    }
}