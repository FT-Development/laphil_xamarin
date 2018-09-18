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
using Xamarin.Forms;
using CoreLocation;
using UrbanAirship;

namespace LAPhil.iOS
{
    public partial class ConcertEventViewController : UIViewController
    {
        Event[] eventList;
        EventComparer eventComparer = new EventComparer();
        //Harish_A3 
        EventService eventService = ServiceContainer.Resolve<EventService>();

        bool isOpenDetialScreen = false;
        bool isDisplayAlert = false;

        //Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
        public ConcertEventViewController(IntPtr handle) : base(handle)
        {
        }

        private GimbalManager gimbalManager;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.isOpenDetialScreen = false;
            this.isDisplayAlert = false;

            this.NavigationController.SetNavigationBarHidden(false, false);

            //Harish_A3 
            eventService
            .UpcomingEvents()
            .Subscribe((Event[] events) => {
                DispatchQueue.MainQueue.DispatchAsync(() => {
                    _ = GetEventList(events);
                });
            });

            UAirship.Location().LocationUpdatesEnabled = true;

        }



        public async void setupGimbal()
        {
            await Task.Delay(5000);

            #if __HB__
                string APIKey = "b2381352-17c9-4d7d-a7ad-1965da52d507";
            #else
                string APIKey = "7dae4172-5c02-49a0-bbaa-a3ec3e845508";
            #endif
            Console.WriteLine("setupGimbal APIKey : {0} ", APIKey);


            //GIMBAL - encapsulate gimbal service management and ios location into following class
            gimbalManager = new GimbalManager();
            //need an empty dictionary for the overload...just doesn't seem right, does it?
            var dict = NSDictionary.FromObjectAndKey((NSString)"nada", (NSString)"nidi");
            //sub out your api key here
            //GimbalFramework.Gimbal.SetAPIKey("YOUR_API_KEY_HERE", dict);
            GimbalFramework.Gimbal.SetAPIKey(APIKey, dict);
            //if app was previously authorized, start up gimbal services
            if (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways)
            {
                gimbalManager.Start();
            }
            //request always auth for location - need a listener since this happens async on start esp on first run
            CLLocationManager manager = new CLLocationManager();
            manager.Delegate = gimbalManager;
            manager.RequestAlwaysAuthorization();

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

        private async Task GetEventList(Event[] obj)
        {
            if (await ShouldReloadData(obj) == false)
                return;

            eventList = obj;

            if (eventList.Length > 0)
            {
                this.lblTitle.Text = "Upcoming Events";
                ConcertsListVieww.Source = new DataSource(this, eventList);
                ConcertsListVieww.ReloadData();
                setupGimbal();
            }
            else
            {
                this.lblTitle.Text = "No Upcoming Events found.";
                setupGimbal();

            }
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.TitleView = new UIImageView(UIImage.FromFile("AppLogo/AppLogoForNav.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate));
            this.NavigationItem.TitleView.TintColor = UIColor.White;

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };
            this.btnSeeCalendar.Layer.BorderWidth = 1;
            this.btnSeeCalendar.Layer.BorderColor = UIColor.FromRGB(red: 26, green: 150, blue: 212).CGColor;
            this.btnSeeCalendar.TouchUpInside += showCalendar;

            ConcertsListVieww.RegisterNibForCellReuse(UINib.FromName("ConcertsTableViewCell", null), "ConcertsTableViewCell");

            ConcertsListVieww.RowHeight = 332;
            ConcertsListVieww.EstimatedRowHeight = 332;
            ConcertsListVieww.WeakDelegate = this;



        }

        private void showCalendar(object sender, EventArgs e)
        {
            //Clear Filter Selected value
            NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEventType");
            //Clear Filter Start & End Selected Date
            NSUserDefaults.StandardUserDefaults.RemoveObject("selectedStartDate");
            NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEndDate");

            var storyboard = UIStoryboard.FromName("Main", null);
            var concertsCalendarTableViewController = storyboard.InstantiateViewController("ConcertsCalendarTableViewController") as ConcertsCalendarTableViewController;
            concertsCalendarTableViewController.filterEventType = "";
            NavigationController.PushViewController(concertsCalendarTableViewController, true);
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);

            if((this.isOpenDetialScreen == false) && (eventList.Length > 0))
            {
                this.isOpenDetialScreen = true;
                var selectedEvent = eventList[index];
                var storyboard = UIStoryboard.FromName("Main", null);

                if (selectedEvent.ShouldOverrideDetails()) {

                    var vc = (GenericWebViewController)storyboard.InstantiateViewController("GenericWebViewController");
                    vc.pageTitle = selectedEvent.Program.Name;
                    vc.url = selectedEvent.GetOverrideUrl();
                    NavigationController.PushViewController(vc, true);

                }else {

                    var programNotesViewController = (ProgramViewController)storyboard.InstantiateViewController("ProgramViewController");
                    programNotesViewController.Event = selectedEvent;
                    NavigationController.PushViewController(programNotesViewController, true);

                }

            }

            return null;
        }

        public UIViewController openBuyNow(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);
            var eventData = eventList[index] as Event;

            if (eventData.ShouldOverrideDetails()) {

                var storyboard = UIStoryboard.FromName("Main", null);
                var vc = (GenericWebViewController)storyboard.InstantiateViewController("GenericWebViewController");
                vc.pageTitle = eventData.Program.Name;
                vc.url = eventData.GetOverrideUrl();
                NavigationController.PushViewController(vc, true);

            }
            else if (eventData.BuyUrl == null)
            {
                if(this.isDisplayAlert == false)
                {
                    this.isDisplayAlert = true;
                    var okAlertController = UIAlertController.Create("No events available", "Please select another range.", UIAlertControllerStyle.Alert);
                    //Add Actions
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert => {
                        this.isDisplayAlert = false;
                    }));
                    PresentViewController(okAlertController, true, null);
                }
            }
            else
            {
                Device.OpenUri(new Uri(eventData.BuyUrl));

                Dictionary<object, object> parameters = new Dictionary<object, object>();
                parameters.Add("Program", eventData.Program.Name);
                Firebase.Analytics.Analytics.LogEvent("BuyNow", parameters);

            }
            return null;
        }


        class DataSource : UITableViewSource
        {
            EventService eventService = ServiceContainer.Resolve<EventService>();
            TimeService timeService = ServiceContainer.Resolve<TimeService>();

            static readonly NSString CellIdentifier = new NSString("ConcertsTableViewCell");
            private ConcertEventViewController _concertsViewController;

            //Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
            Event[] eventList = new Event[]{};

            public DataSource(UIViewController parentViewController, Event[] tmpEventList)
            {
                _concertsViewController = parentViewController as ConcertEventViewController;
                eventList = tmpEventList;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return eventList.Length;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {

                ConcertsTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as ConcertsTableViewCell;

                var indexRow = indexPath.Row;
                var eventData = eventList[indexRow] as Event;

                Console.WriteLine("eventData Buy URl : {0}",eventData.BuyUrl);

                var localTime = eventData.StartTime.ToLocalTime();
                var date1 = localTime.ToString("MMM dd");
                var date2 = localTime.ToString($"ddd - {timeService.TimeFormat}");

                cell.lblDate1.Text = date1;
                cell.lblDate2.Text = date2;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;

                cell.lblTitle.AttributedText = ($"<b>{eventData.Program.Name}</b>")
                    .HtmlAttributedString(matchingLabel: cell.lblTitle);
                
                cell.Accessories.Hidden = true;
                cell.Accessories.RemoveArrangedSubview(cell.AccessoryMessageLabel);
                cell.Accessories.RemoveArrangedSubview(cell.AccessoryBadgeView);
                cell.AccessoryMessageLabel.Hidden = true;
                cell.AccessoryBadgeView.Hidden = true;


                if(eventData.Program.ExtraMessage != null)
                    cell.Accessories.Hidden = false;

                if (eventData.Program.ExtraMessage != null)
                {
                    cell.AccessoryMessageLabel.Hidden = false;
                    cell.Accessories.AddArrangedSubview(cell.AccessoryMessageLabel);
                    cell.AccessoryMessageLabel.AttributedText = eventData
                        .Program
                        .ExtraMessage.ToUpper()
                        .HtmlAttributedString(matchingLabel: cell.AccessoryMessageLabel);
                }

                var imgConcertRect = cell.imgConcert.Frame;
                var screenScale = UIScreen.MainScreen.Scale;

                if (localTime == DateTimeOffset.MinValue)
                {
                    cell.lblDate1.Hidden = true;
                    cell.lblDate2.Hidden = true;
                } 
                else 
                {
                    cell.lblDate1.Hidden = false;
                    cell.lblDate2.Hidden = false;
                }

                Task.Run(async () =>
                {
                    var size = new System.Drawing.Size(
                        width: (int)(imgConcertRect.Width * screenScale),
                        height: (int)(imgConcertRect.Height * screenScale)
                    );

                    var bytes = await eventService.GetEventImage3x2Bytes(eventData, size: size);

                    if (bytes == null)
                    {
                        DispatchQueue.MainQueue.DispatchAsync(() =>
                        {
                            cell.imgConcert.Hidden = true;
                        });

                        return;
                    }

                    var image = UIImage.LoadFromData(
                        NSData.FromArray(bytes)
                    );

                    DispatchQueue.MainQueue.DispatchAsync(() =>
                    {
                        cell.imgConcert.Hidden = false;
                        cell.imgConcert.Image = image;
                        cell.imgConcert.ContentMode = UIViewContentMode.ScaleAspectFill;

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

                cell.btnSeeDetails.Layer.BorderWidth = 1;
                cell.btnSeeDetails.SetTitleColor(UIColor.FromRGB(red: 26, green: 150, blue: 212), UIControlState.Normal);
                cell.btnSeeDetails.Layer.BorderColor = UIColor.FromRGB(red: 26, green: 150, blue: 212).CGColor;
                cell.btnSeeDetails.Tag = indexPath.Row;
                cell.btnSeeDetails.TouchUpInside -= actionShowDetail;
                cell.btnSeeDetails.TouchUpInside += actionShowDetail;

                cell.btnOpenDetail.Tag = indexPath.Row;
                cell.btnOpenDetail.TouchUpInside -= actionShowDetail;
                cell.btnOpenDetail.TouchUpInside += actionShowDetail;

                cell.btnBuyNow.Tag = indexPath.Row;
                cell.btnBuyNow.SetTitle("PURCHASE OPTIONS", UIControlState.Normal);
                cell.btnBuyNow.SetTitle("PURCHASE OPTIONS", UIControlState.Highlighted);
                cell.btnBuyNow.TouchUpInside -= actionBuyNow;
                cell.btnBuyNow.TouchUpInside += actionBuyNow;

                return cell;
            }

            public void actionBuyNow(object sender, EventArgs e)
            {
                var selectedButtonIndex = sender as UIButton;
                Console.WriteLine("Selected DETAIL indexPath Row {0}", selectedButtonIndex.Tag);
                _concertsViewController.openBuyNow((int)selectedButtonIndex.Tag);

            }

            public void actionShowDetail(object sender, EventArgs e)
            {
                var selectedButtonIndex = sender as UIButton;
                Console.WriteLine("Selected DETAIL indexPath Row {0}", selectedButtonIndex.Tag);
                _concertsViewController.ViewControllerAtIndex((int)selectedButtonIndex.Tag);
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
            }
        }
    }
}