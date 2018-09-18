using Foundation;
using CoreFoundation;
using System;
using UIKit;
using CoreGraphics;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive.Linq;
using LAPhil.Events;
using LAPhil.Application;
using LAPhil.Platform;
using LAPhilShared;
using LAPhil.iOS.TableViewCell;
using System.Diagnostics.Contracts;
using System.Collections; 

namespace LAPhil.iOS
{
    public partial class EventTypesTableViewController : UITableViewController
    {
        ArrayList itemFilterData = new ArrayList(); 
        ArrayList itemFilterDataFinal = new ArrayList();
        ArrayList eventTypes = new ArrayList();


        int selectedIndex = -1;
        EventComparer eventComparer = new EventComparer();
        EventService eventService = ServiceContainer.Resolve<EventService>();

        [Outlet]
        public UIKit.UIButton btnApplyFilter { get; set; }

        public EventTypesTableViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationController.NavigationBar.Hidden = false;
            this.NavigationController.NavigationBar.BackgroundColor = UIColor.Clear;
            this.NavigationItem.Title = "Choose Event Type(s)";
            this.ConfigureDefaultBackButton();
            this.View.BackgroundColor = UIColor.FromRGB(35, 31, 32);

            btnApplyFilter.TouchUpInside += actApplyFilter;
            //Harish_15M
            //eventService.CurrentEvents()
            //.Subscribe((Event[] obj) => _ = GetEventList(obj));
            GetAllEventList();
        }

        private void GetAllEventList()
        {
            Event[] obj = Singleton.Instance.events;

            Contract.Ensures(Contract.Result<Task>() != null);
            //if (await ShouldReloadData(obj) == false)
                //return;

            itemFilterDataFinal.Clear();
            for (int i = 0; i < obj.Length; i++)
            {
                //Harish_15M
                if(obj[i].Program.categories != null)
                {
                    Console.WriteLine("firstIndex.Program.Categories :::::::::::: " + obj[i].Program.categories.Count);
                    for (int k = 0; k < obj[i].Program.categories.Count; k++)
                    {
                        var eventTypeName = obj[i].Program.categories[k];
                                                  
                        if (!itemFilterDataFinal.Contains(eventTypeName))
                        {
                            var objEvent = new NSMutableDictionary();

                            objEvent.SetValueForKey(new NSString(eventTypeName), new NSString("title"));
                            objEvent.SetValueForKey(new NSString("false"), new NSString("selected"));

                            itemFilterDataFinal.Add(eventTypeName);
                            eventTypes.Add(objEvent);
                        }

                    }
                }


            }

            //itemFilterDataFinal.Clear();
            //for (int i = 0; i < obj.Length; i++)
            //{
            //    //Console.WriteLine(">>>> " + obj[i].TicketingSystemId + " ----- "+ obj[i].TicketingSystemType);
            //    if (!itemFilterDataFinal.Contains(obj[i].TicketingSystemType) && obj[i].TicketingSystemType != "")
            //    {
            //        itemFilterDataFinal.Add(obj[i].TicketingSystemType);
            //        //Console.WriteLine("> ADD >>>> " + obj[i].TicketingSystemId + " ----- " + obj[i].TicketingSystemType);
            //    }
            //}
            //Console.WriteLine("itemFilterDataFinal count : " + itemFilterDataFinal.Count);

            this.eventListview.RegisterNibForCellReuse(UINib.FromName("EventTypesTableViewCell", null), "EventTypesTableViewCell");
            this.eventListview.Source = new DataSource(this, eventTypes);
            this.eventListview.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.SetNavigationBarHidden(false, false);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            if(this.selectedIndex != -1)
            {
                Console.WriteLine("ViewDidDisappear : " + itemFilterDataFinal[this.selectedIndex]);
            }
        }

        private async Task EventsTypeList(Event[] obj)
        {
            if (await ShouldReloadData(obj) == false)
                return;
        }

        private Task<bool> ShouldReloadData(Event[] obj)
        {
            Event[] eventList = new Event[] { };

            if (eventList == null)
                return Task.FromResult(true);

            return Task.Run(() =>
            {
                var deletions = eventList.Except(obj, eventComparer).Count();
                var insertions = obj.Except(eventList, eventComparer).Count();

                if (deletions == 0 && insertions == 0)
                    return false;

                return true;
            });
        }

        private void actionBack(object sender, EventArgs e)
        {
            this.NavigationController.PopViewController(true);
        }

        private void actApplyFilter(object sender, EventArgs e)
        {
            var selectedEventType = (string)"";
           
            for (int k = 0; k < eventTypes.Count; k++)
            {
                var dict = eventTypes[k] as NSMutableDictionary;
                if (dict.ValueForKey(new NSString("selected")).ToString() == "true")
                {
                    selectedEventType = selectedEventType + dict.ValueForKey(new NSString("title")).ToString() + ",";
                }
            }
            if(selectedEventType != "")
            {
                selectedEventType = selectedEventType.Remove(selectedEventType.Length - 1, 1);    
            }


            if(selectedEventType.Length > 0)
            {
                NSUserDefaults.StandardUserDefaults.SetString(selectedEventType, "selectedEventType");
                NSUserDefaults.StandardUserDefaults.Synchronize();
                //this.DismissViewController(true, null);
                this.NavigationController.PopViewController(true);
            }else{
                var okAlertController = UIAlertController.Create("No events available", "Please select another range.", UIAlertControllerStyle.Alert);
                //Add Actions
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert => {
                    
                }));
                PresentViewController(okAlertController, true, null);
            }
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("_eventTypesTableViewController ViewControllerAtIndex index : {0} ", index);
            this.selectedIndex = index;

            var dict = eventTypes[this.selectedIndex] as NSMutableDictionary;
            if(dict.ValueForKey(new NSString("selected")).ToString() == "true"){
                dict.SetValueForKey(new NSString("false"), new NSString("selected"));
            }else{
                dict.SetValueForKey(new NSString("true"), new NSString("selected"));
            }
            eventTypes[this.selectedIndex] = dict;
            this.eventListview.ReloadData();

            //var selectedEventType = (string)itemFilterDataFinal[this.selectedIndex];
            //NSUserDefaults.StandardUserDefaults.SetString(selectedEventType, "selectedEventType");
            //NSUserDefaults.StandardUserDefaults.Synchronize();
            //Retrieve
            //NSUserDefaults.StandardUserDefaults.StringForKey("selectedEventType");
            //Clear:
            //NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEventType");
            //Task.Delay(500);
            //this.NavigationController.PopViewController(true);
            //Harish_M31
           // NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"Notification_FilterEventType", null);
            //Task.Delay(500);
            //this.DismissViewController(true, null);


            return null;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        class DataSource : UITableViewSource
        {
            static readonly NSString CellIdentifier = new NSString("EventTypesTableViewCell");
            int selectedIndex = -1;

            private EventTypesTableViewController _eventTypesTableViewController;
            ArrayList itemFilterDataFinal = new ArrayList(); 

            public DataSource(UIViewController parentViewController, ArrayList items)
            {
                _eventTypesTableViewController = parentViewController as EventTypesTableViewController;
                itemFilterDataFinal = items;
            }

            // Customize the number of sections in the table view.
            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return itemFilterDataFinal.Count;
            }

            public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            {
                return 0;
            }

            public override nfloat GetHeightForFooter(UITableView tableView, nint section)
            {
                return 0;
            }

            //public override UIView GetViewForHeader(UITableView tableView, nint section)
            //{
            //    var headerView = new UIView(new CoreGraphics.CGRect(15, 15, ((float)tableView.Bounds.Width - 30), 1));
            //    headerView.BackgroundColor = UIColor.Clear;
            //    UIImage img = UIImage.FromFile("ConcertsCalendar/filterLine.png");
            //    var imgView = new UIImageView(img, img) { Frame = headerView.Frame };
            //    imgView.Alpha = (float)0.50;
            //    headerView.AddSubview(imgView);
            //    return headerView;
            //}

            // Customize the appearance of table view cells.
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                EventTypesTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as EventTypesTableViewCell;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                var dict = itemFilterDataFinal[indexPath.Row] as NSMutableDictionary;
                cell.labelTitle.Text = dict.ValueForKey(new NSString("title")).ToString();//itemFilterDataFinal[indexPath.Row].ToString();

                //if(indexPath.Row == selectedIndex)
                if (dict.ValueForKey(new NSString("selected")).ToString() == "true")
                {
                    cell.viewEventTitle.BackgroundColor= UIColor.FromRGB(26, 150, 212);
                }else
                {
                    cell.viewEventTitle.BackgroundColor = UIColor.FromRGB(63, 63, 65);
                }
                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                EventTypesTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as EventTypesTableViewCell;
                cell.viewEventTitle.BackgroundColor = UIColor.Blue;
                selectedIndex = indexPath.Row;
                _eventTypesTableViewController.ViewControllerAtIndex(indexPath.Row);
            }
        }
    }
}
