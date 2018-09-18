using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using LAPhilShared;

namespace LAPhil.iOS
{
    public partial class ConcertsCalendarFilterTableViewController : UITableViewController
    {
        List<string> itemData;
        //Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

        public ConcertsCalendarFilterTableViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.DarkGray;

            this.NavigationItem.Title = "Calendar Filter";
            var customLeftButton = new UIBarButtonItem();
            var customRightButton = new UIBarButtonItem(
                UIImage.FromFile("Others/crosWhite.png"),
            UIBarButtonItemStyle.Plain,
            (s, e) => {
                System.Diagnostics.Debug.WriteLine("Right button tapped");
                this.NavigationController.PopViewController(true);
            });
            customRightButton.TintColor = UIColor.White;
            this.NavigationItem.LeftBarButtonItem = customLeftButton;
            this.NavigationItem.RightBarButtonItem = customRightButton;


            itemData = LAPhilShared.SharedClass.GetFilterData();
            filterListview.RegisterNibForCellReuse(UINib.FromName("ConcertsCalendarFilterTableViewCell", null), "ConcertsCalendarFilterTableViewCell");
            filterListview.Source = new DataSource(this, itemData);

        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);

            if(index == 0)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var chooseEventTypeTableViewController = storyboard.InstantiateViewController("ChooseEventTypeTableViewController") as ChooseEventTypeTableViewController;
                NavigationController.PushViewController(chooseEventTypeTableViewController, true);
            }else
            {
                //TODO: Will will when we integrate Calendar 
                //var storyboard = UIStoryboard.FromName("Main", null);
                //var chooseEventTypeTableViewController = storyboard.InstantiateViewController("CalendarViewController") as CalendarViewController;
                //NavigationController.PushViewController(chooseEventTypeTableViewController, true);
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
            static readonly NSString CellIdentifier = new NSString("ConcertsCalendarFilterTableViewCell");

            private ConcertsCalendarFilterTableViewController _concertsCalendarFilterTableViewController;

            List<string> tabledata;

            public DataSource(UIViewController parentViewController, List<string> items)
            {
                _concertsCalendarFilterTableViewController = parentViewController as ConcertsCalendarFilterTableViewController;
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
                
                ConcertsCalendarFilterTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as ConcertsCalendarFilterTableViewCell;
                cell.lblTitle.Text = tabledata[indexPath.Row].ToString();

                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);
                tableView.DeselectRow(indexPath, true);
                if(indexPath.Row > 0)
                {
                    _concertsCalendarFilterTableViewController.ViewControllerAtIndex(indexPath.Row);    
                }
            }
        }
    }
}