using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using LAPhilShared;
using System.Threading.Tasks;

namespace LAPhil.iOS
{


    public partial class ConcertsCalendarFilterViewController : UIViewController
    {

        [Outlet]
        public UIKit.UIButton btnClose { get; set; }

        [Outlet]
        public UIKit.UIButton btnSeeConcerts { get; set; }

        [Outlet]
        public UIKit.UITableView filterListview { get; set; }

        [Outlet]
        public UIKit.UIView viewSeeConcerts { get; set; }

        [Outlet]
        public UIKit.UIView viewTitle { get; set; }

        List<string> itemData;

        public ConcertsCalendarFilterViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.FromRGB(35, 31, 32);
            this.filterListview.BackgroundColor = UIColor.FromRGB(35, 31, 32);
            btnClose.TouchUpInside += CloseView;
            //TODO:19M
            btnSeeConcerts.TouchUpInside += actSeeConcerts;

            itemData = LAPhilShared.SharedClass.GetFilterData();
            this.filterListview.RegisterNibForCellReuse(UINib.FromName("ConcertsCalendarFilterTableViewCell", null), "ConcertsCalendarFilterTableViewCell");
            this.filterListview.Source = new DataSource(this, itemData);

        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.NavigationBar.Hidden = true;

            this.funIsDisplaySeeContent();

            this.filterListview.ReloadData();
        }

        public void funIsDisplaySeeContent()
        {
            var selectedEventType = NSUserDefaults.StandardUserDefaults.StringForKey("selectedEventType");
            var dateStart = NSUserDefaults.StandardUserDefaults.StringForKey("selectedStartDate");
            var dateEnd = NSUserDefaults.StandardUserDefaults.StringForKey("selectedEndDate");

            if (selectedEventType == null && dateStart == null && dateEnd == null)
            {
                this.btnSeeConcerts.Hidden = false;
                //NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"Notification_FilterEventType", null);
                //Task.Delay(500);
            }
            else
            {
                //Harish_M31    this.btnSeeConcerts.Hidden = false;
                this.btnSeeConcerts.Hidden = false;
            }
        }

        private void actSeeConcerts(object sender, EventArgs e)
        {
            Console.WriteLine("actSeeConcerts ");
            NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"Notification_FilterEventType", null);             Task.Delay(500);
            this.DismissViewController(true, null);
        }


        private void CloseView(object sender, EventArgs e)
        {
            Console.WriteLine("Pop CloseView ");
            //Clear
            NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEventType");
            NSUserDefaults.StandardUserDefaults.RemoveObject("selectedStartDate");
            NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEndDate");
            NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"Notification_FilterEventType", null);
            Task.Delay(500);
            this.DismissViewController(true, null);

        }


        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);

            if (index == -1)
            {
                //Clear
                NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEventType");
                this.filterListview.ReloadData();
            }if (index == -2)   // Start & End date unselected
            {
                //Clear
                NSUserDefaults.StandardUserDefaults.RemoveObject("selectedStartDate");
                NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEndDate");
                this.filterListview.ReloadData();

            }else if (index == 0)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var eventTypesTableViewController = storyboard.InstantiateViewController("EventTypesTableViewController") as EventTypesTableViewController;
                NavigationController.PushViewController(eventTypesTableViewController, true);
            }
            else if (index == 1)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var chooseEventTypeTableViewController = storyboard.InstantiateViewController("CustomSelectionController") as CustomSelectionController;
                NavigationController.PushViewController(chooseEventTypeTableViewController, true);

            }

            this.funIsDisplaySeeContent();
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

            private ConcertsCalendarFilterViewController _concertsCalendarFilterTableViewController;

            List<string> tabledata;

            public DataSource(UIViewController parentViewController, List<string> items)
            {
                _concertsCalendarFilterTableViewController = parentViewController as ConcertsCalendarFilterViewController;
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
                var selectedEventType = NSUserDefaults.StandardUserDefaults.StringForKey("selectedEventType");
                Console.WriteLine("selectedEventType : " + selectedEventType);


                var dateStart = NSUserDefaults.StandardUserDefaults.StringForKey("selectedStartDate");
                var dateEnd = NSUserDefaults.StandardUserDefaults.StringForKey("selectedEndDate");


                if (selectedEventType != null && indexPath.Row == 0)
                {
                    var selectedEvent = NSUserDefaults.StandardUserDefaults.StringForKey("selectedEventType");
                    cell.lblTitle.Text = selectedEvent;
                    cell.viewBackground.BackgroundColor = UIColor.FromRGB(26, 150, 212);
                    cell.btnDetail.TouchUpInside += EventTypeUnSelect;
                    cell.btnDetail.Hidden = false;
                    cell.imgDetail.Hidden = true;
                }else if (dateStart != null && dateEnd != null && indexPath.Row == 1)
                {
                    var selectedStartEndDate = dateStart + " -> " + dateEnd;
                    cell.lblTitle.Text = selectedStartEndDate;
                    cell.viewBackground.BackgroundColor = UIColor.FromRGB(26, 150, 212);
                    cell.btnDetail.TouchUpInside += DateUnSelect;
                    cell.btnDetail.Hidden = false;
                    cell.imgDetail.Hidden = true;
                }else
                {
                    cell.lblTitle.Text = tabledata[indexPath.Row];
                    cell.viewBackground.BackgroundColor = UIColor.Clear;
                    cell.btnDetail.Hidden = true;
                    cell.imgDetail.Hidden = false;
                }

                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);
                tableView.DeselectRow(indexPath, true);
                _concertsCalendarFilterTableViewController.ViewControllerAtIndex(indexPath.Row);
            }

            private void EventTypeUnSelect(object sender, EventArgs e)
            {
                Console.WriteLine("Pop EventTypeUnSelect ");
                _concertsCalendarFilterTableViewController.ViewControllerAtIndex(-1);
            }


            private void DateUnSelect(object sender, EventArgs e)
            {
                Console.WriteLine("Pop DateUnSelect ");
                _concertsCalendarFilterTableViewController.ViewControllerAtIndex(-2);
            }


        }
    }
}
