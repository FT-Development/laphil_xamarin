using Foundation;
using CoreFoundation;
using CoreGraphics;
using UIKit;
using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Threading;
using LAPhil.Events;
using LAPhil.Application;
using LAPhil.Platform;
using System.Collections;
using System.Diagnostics.Contracts;
using LAPhil.Urls;

namespace LAPhil.iOS
{
    
    public partial class ConcertsCalendarTableViewController : UITableViewController
    {
        Event[] eventList = new Event[]{};

        LAPhilUrlService urls = ServiceContainer.Resolve<LAPhilUrlService>();

        Dictionary<String, List<Event>> organizedEvents;

        NSObject observer;
        public String filterEventType = "";
        UIImageView loader;
        LoadingService loadingService = ServiceContainer.Resolve<LoadingService>();

        EventService eventService = ServiceContainer.Resolve<EventService>();
        EventComparer eventComparer = new EventComparer();

        Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

        public ConcertsCalendarTableViewController(IntPtr handle) : base(handle)
        {
        }

        int isFilterDataFound = 0;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //this.viewHeader.Hidden = false;
            //this.ConcertsCalendarListView.TableHeaderView = this.viewHeader;

            this.isFilterDataFound = 1;

            this.View.BackgroundColor = UIColor.Black;

            //TODO: Hidden for current build after that will display Filter button - Harish_A10
            this.btnFilter.Hidden = false;

            this.btnFilter.Layer.BorderWidth = 1;
            this.btnFilter.Layer.BorderColor = UIColor.Gray.CGColor;

            //this.btnBack.TouchUpInside += actionBack;

            this.NavigationItem.Title = "Calendar";
            this.ConfigureDefaultBackButton();


            this.btnFilter.TouchUpInside += actionFilter;
            //View.BringSubviewToFront(this.btnFilter);
            // TODO: Finish Filters -- harish Chouhan
            //this.viewFilter.RemoveFromSuperview();

            loader = loadingService.CreateIndeterminateLoader();
            loader.Hidden = true;
            View.AddSubview(loader);
            loadingService.LayoutAtTopOfViewController(loader, this);
            View.BringSubviewToFront(loader);

            ShowLoader(true);
            LoadData();

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (observer != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
                observer = null;
            }
            if(this.isFilterDataFound == 0)
            {
                this.ConcertsCalendarListView.Hidden = true;

                var strAppTitle = urls.AppTitle;
                var okAlertController = UIAlertController.Create("No events available", "Please select another range.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert =>
                {
                    var storyboard = UIStoryboard.FromName("Main", null);
                    var concertsCalendarFilterViewController = storyboard.InstantiateViewController("filterNavigationView") as filterNavigationView;
                    this.PresentModalViewController(concertsCalendarFilterViewController, true);
                    observer = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"Notification_FilterEventType", notification_LoadData);

                }));
                this.PresentViewController(okAlertController, true, null);
            }else
            {
                this.ConcertsCalendarListView.Hidden = false;
            }
        }

        void notification_LoadData(NSNotification obj)         {             Console.WriteLine(" CALL notification_LoadData");
            filterEventType = NSUserDefaults.StandardUserDefaults.StringForKey("selectedEventType");
            Console.WriteLine("filterEventType : " + filterEventType);
            var dateStart = NSUserDefaults.StandardUserDefaults.StringForKey("selectedStartDate");
            var dateEnd = NSUserDefaults.StandardUserDefaults.StringForKey("selectedEndDate");

            DateTime dateTimeStart = Convert.ToDateTime(dateStart);
            DateTime dateTimeEND = Convert.ToDateTime(dateEnd);

            if (dateStart == dateEnd) {
                dateTimeEND = dateTimeEND.AddDays(1);
            }

            Console.WriteLine("dateStart : {0} ---> dateEnd : {1} ",dateStart,dateEnd);

            ArrayList list = new ArrayList();
            Event[] objAllEvent = Singleton.Instance.events;
            Console.WriteLine("Obj Count : " + objAllEvent.Length);

            this.isFilterDataFound = 0;

            if(filterEventType != null && dateStart != null && dateEnd != null) // FIlter & Date Apply
            {
                for (int i = 0; i < objAllEvent.Length; i++)
                {
                    if (objAllEvent[i].Program.categories != null)
                    {
                        var isFound = false;
                        for (int j = 0; j < objAllEvent[i].Program.categories.Count; j++){
                            var cate = objAllEvent[i].Program.categories[j].ToString();
                            if (filterEventType.ToString().IndexOf(cate) > -1){
                                isFound = true;
                            }
                        }
                        if (isFound && (objAllEvent[i].StartTime >= dateTimeStart && objAllEvent[i].StartTime <= dateTimeEND) )
                        {
                            Event tmpObj = objAllEvent[i];
                            list.Add(tmpObj);
                        }
                    }
                }
            }else if (filterEventType != null && dateStart == null && dateEnd == null)  // Only FIlter Apply
            {
                for (int i = 0; i < objAllEvent.Length; i++)
                {
                    //Harish_15M
                    if (objAllEvent[i].Program.categories != null)
                    {
                        var isFound = false;
                        for (int j = 0; j < objAllEvent[i].Program.categories.Count; j++)
                        {
                            var cate = objAllEvent[i].Program.categories[j].ToString();
                            if (filterEventType.ToString().IndexOf(cate) > -1)
                            {
                                isFound = true;
                            }
                        }
                        if (isFound)//objAllEvent[i].Program.categories.Contains(filterEventType))
                        {
                            Event tmpObj = objAllEvent[i];
                            list.Add(tmpObj);
                        }
                    }
                }
            }else if (filterEventType == null && dateStart != null && dateEnd != null)  // only Date Apply
            {
                for (int i = 0; i < objAllEvent.Length; i++)
                {
                    //Harish_15M
                    if (objAllEvent[i].StartTime >= dateTimeStart && objAllEvent[i].StartTime <= dateTimeEND)
                    {
                        Event tmpObj = objAllEvent[i];
                        list.Add(tmpObj);
                    }
                }
            }else
            {
                this.isFilterDataFound = 1;
                DispatchQueue.MainQueue.DispatchAsync(() => {
                    _ = GetEventList(Singleton.Instance.events);
                });
            }

            if(list.Count > 0)
            {
                this.isFilterDataFound = 1;
                Event[] arrayTmp = list.ToArray(typeof(Event)) as Event[];
                Console.WriteLine("Obj Count : " + arrayTmp.Length);
                DispatchQueue.MainQueue.DispatchAsync(() => {
                    _ = GetEventList(arrayTmp);
                });
            }
        }

        void LoadData()
        {
            //Get All Events
            eventService.CurrentEvents()
                        .Subscribe((Event[] obj) => _ = GetEventList(obj));
        }

        private Task<bool> ShouldReloadData(Event[] obj)
        {
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

        void ShowLoader(bool visible)
        {
            if (visible)
            {
                loader.Hidden = false;
                loader.StartAnimating();
            }else 
            {
                loader.Hidden = true;
                loader.StopAnimating();
            }
        }

        private async Task GetEventList(Event[] obj)
        {

            if (await ShouldReloadData(obj) == false)
            {
                return;
            }

            if(Singleton.Instance.events == null)
            {
                Singleton.Instance.events = obj;    
            }

            eventList = obj;
            var firstIndex = eventList[0];

            var cellViewNib = UINib.FromName("ConcertsCalendarTableViewCell", bundleOrNil: NSBundle.MainBundle);
            var headerViewNib = UINib.FromName("CalendarHeader", bundleOrNil: NSBundle.MainBundle);
            Console.WriteLine("GetMyEventData : {0}", firstIndex.Program.Name);

            ConcertsCalendarListView.RegisterNibForCellReuse(nib: cellViewNib, reuseIdentifier: "ConcertsCalendarTableViewCell");
            ConcertsCalendarListView.RegisterNibForHeaderFooterViewReuse(nib:headerViewNib, reuseIdentifier: "ConcertsCalendarTableViewHeader");

            ConcertsCalendarListView.RowHeight = UITableView.AutomaticDimension;
            ConcertsCalendarListView.EstimatedRowHeight = 120;
            ConcertsCalendarListView.SectionHeaderHeight = 50;

            organizedEvents = new Dictionary<String, List<Event>>();

            await Task.Run(() =>
            {
                foreach (Event evt in eventList)
                {

                    var dateTime = evt.StartTime.ToLocalTime();
                    var key = dateTime.Year + "-" + dateTime.Month;
                    List<Event> list;
                    if (organizedEvents.ContainsKey(key))
                    {
                        list = organizedEvents[key];
                    }
                    else
                    {
                        list = new List<Event>();
                    }
                    list.Add(evt);
                    organizedEvents[key] = list;

                }

            });

            ConcertsCalendarListView.Source = new DataSource(this, organizedEvents);
            ConcertsCalendarListView.ReloadData();
            ShowLoader(false);

            DispatchQueue.MainQueue.DispatchAsync(() => {
                Console.WriteLine("EventList.Count {0} GroupedEvents.Count {1} ConcertsCalendarListView.NumberOfSections() {2}", eventList.Length, organizedEvents.Count, ConcertsCalendarListView.NumberOfSections());
                // this is needed to update a specific tableview's headerview layout on main queue otherwise it's won't update perfectly cause reloaddata() is called
                ConcertsCalendarListView.BeginUpdates();
                ConcertsCalendarListView.ReloadSections(NSIndexSet.FromNSRange(new NSRange(0, ConcertsCalendarListView.NumberOfSections() - 1)), UITableViewRowAnimation.None);
                ConcertsCalendarListView.EndUpdates();
            });

        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return (nfloat) 40.0;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if (eventList != null)
            {
                Console.WriteLine("EventList.Count {0} EventList[{1}]", eventList.Length, section);
                if (section < eventList.Length)
                {
                    var currentSection = eventList[section];
                    if (currentSection != null)
                    {
                        var textMonthYear = currentSection.StartTime.Date.ToString("MMMMMMMMMMMMM yyyy");
                        var header = (CalendarHeader)tableView.DequeueReusableHeaderFooterView("ConcertsCalendarTableViewHeader");
                        header.Label.AttributedText = new NSAttributedString(textMonthYear);
                        return header;
                    }   
                }
            }
            return null;
        }

        private void actionBack(object sender, EventArgs e)
        {
            this.NavigationController.PopViewController(true);

        }

        private void actionFilter(object sender, EventArgs e)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var concertsCalendarFilterViewController = storyboard.InstantiateViewController("filterNavigationView") as filterNavigationView;
            this.PresentModalViewController(concertsCalendarFilterViewController, true);
            observer = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"Notification_FilterEventType", notification_LoadData); 
        }

        public UIViewController ViewControllerAtIndex(NSIndexPath indexPath)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0}-{1}", indexPath.Section, indexPath.Row);

            var key = organizedEvents.Keys.ToList()[(int)indexPath.Section];
            var selectedEvent = organizedEvents[key][(int)indexPath.Row];
            var storyboard = UIStoryboard.FromName("Main", null);

            if (selectedEvent.ShouldOverrideDetails()) 
            {
                var vc = (GenericWebViewController)storyboard.InstantiateViewController("GenericWebViewController");
                vc.pageTitle = selectedEvent.Program.Name;
                vc.url = selectedEvent.GetOverrideUrl();
                NavigationController.PushViewController(vc, true);   
            }
            else 
            {
                var programNotesViewController = storyboard.InstantiateViewController("ProgramViewController") as ProgramViewController;
                programNotesViewController.Event = selectedEvent;
                NavigationController.PushViewController(programNotesViewController, true);
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
            EventService eventService = ServiceContainer.Resolve<EventService>();
            TimeService timeService = ServiceContainer.Resolve<TimeService>();

            static readonly NSString CellIdentifier = new NSString("ConcertsCalendarTableViewCell");

            private ConcertsCalendarTableViewController _ConcertsCalendarTableViewController;
            Dictionary<String, List<Event>> eventList;

            public DataSource(UIViewController parentViewController, Dictionary<String, List<Event>> tmpEventList)
            {
                _ConcertsCalendarTableViewController = parentViewController as ConcertsCalendarTableViewController;
                eventList = tmpEventList;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return eventList.Count;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                var key = eventList.Keys.ToList()[(int)section];
                return eventList[key].Count();
            }

            public override UIView GetViewForHeader(UITableView tableView, nint section)
            {
                var key = eventList.Keys.ToList()[(int)section];
                string []tokens = key.Split('-');
                int year = System.Convert.ToInt32(tokens[0]);
                int month = System.Convert.ToInt32(tokens[1]);
                var textMonthYear = new DateTime(year, month, 1).ToString("MMMMMMMMMMMMM yyyy");

                var header = (CalendarHeader)tableView.DequeueReusableHeaderFooterView("ConcertsCalendarTableViewHeader");
                header.Label.AttributedText = new NSAttributedString(textMonthYear);

                return header;
            }

            public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
            {
                var view = (CalendarHeader)headerView;
                view.BackgroundColor = UIColor.Black;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                ConcertsCalendarTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as ConcertsCalendarTableViewCell;
                var indexRow = indexPath.Row;
                var key = eventList.Keys.ToList()[(int)indexPath.Section];
                var eventData = eventList[key][(int)indexPath.Row];

                var localTime = eventData.StartTime.ToLocalTime();
                cell.Components.RemoveArrangedSubview(cell.lblHouseRules);
                cell.lblHouseRules.Hidden = true;

                var date = localTime.ToString($"ddd, MMM dd - {timeService.TimeFormat}");
                cell.lblDate1.Text = date;

                if (eventData.Program.ExtraMessage != null)
                {
                    cell.Components.AddArrangedSubview(cell.lblHouseRules);
                    cell.lblHouseRules.Text = eventData.Program.ExtraMessage.ToUpper();
                    cell.lblHouseRules.Hidden = false;
                }

                cell.SelectionStyle = UITableViewCellSelectionStyle.None;

                // Available Font Names: 
                // ApercuPro-BoldItalic
                // ApercuPro-LightItalic
                // ApercuPro-BlackItalic
                // ApercuPro-Mono
                // ApercuPro-MonoBold
                // ApercuPro-MediumItalic
                // ApercuPro-Black
                // ApercuPro-Medium
                // ApercuPro-Regular
                // ApercuPro-Light
                // ApercuPro-Italic
                // ApercuPro-Bold

                cell.lblTitle.AttributedText = ($"<b>{eventData.Program.Name}</b>")
                    .HtmlAttributedString(matchingLabel: cell.lblTitle);

                var imgConcertRect = cell.imgConcert.Frame;
                var screenScale = UIScreen.MainScreen.Scale;

                Task.Run(async () =>
                {
                    var size = new System.Drawing.Size(
                        width: (int)(imgConcertRect.Width * screenScale),
                        height: (int)(imgConcertRect.Height * screenScale)
                    );

                    var bytes = await eventService.GetEventImage1x1Bytes(eventData, size: size);

                    if (bytes == null)
                    {
                        BeginInvokeOnMainThread(() => {
                            cell.imgConcert.Hidden = true;
                        });

                        return;
                    }

                    var image = UIImage.LoadFromData(
                        NSData.FromArray(bytes)
                    );

                    DispatchQueue.MainQueue.DispatchAsync(() => {
                        UIView.Transition(
                            withView: cell.imgConcert,
                        duration: 0.25,
                        options: UIViewAnimationOptions.TransitionCrossDissolve,
                        animation: () => {
                            cell.imgConcert.Hidden = false;
                            cell.imgConcert.Image = image;
                        },
                        completion: null);
                    });

                });

                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);
                _ConcertsCalendarTableViewController.ViewControllerAtIndex(indexPath);
            }
        }
    }
}
