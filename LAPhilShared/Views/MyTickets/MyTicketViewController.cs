using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Reactive.Linq;
using Newtonsoft.Json;
using LAPhilShared;
using LAPhil.Auth;
using LAPhil.User;
using LAPhil.Application;
using LAPhil.Events;
using LAPhil.Logging;
using LAPhil.Platform;
using LAPhil.iOS.TableViewCell;



namespace LAPhil.iOS
{
    
    public partial class MyTicketViewController : UIViewController
    {
        Ticket nextTicket;
        Ticket[] myCurrentTickets;
        Event[] eventList;
        EventComparer eventComparer = new EventComparer();
        CompositeDisposable DisposeBag;
        LoginService loginService = ServiceContainer.Resolve<LoginService>();
        TicketsService ticketsService = ServiceContainer.Resolve<TicketsService>();
        EventService eventService = ServiceContainer.Resolve<EventService>();
        TimeService timeService = ServiceContainer.Resolve<TimeService>();
        LoadingService loadingService = ServiceContainer.Resolve<LoadingService>();

        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<MyTicketViewController>();
        UIViewController ticketStatusViewController;
        UIImageView loader;


        public MyTicketViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            DisposeBag = new CompositeDisposable();
            loader = loadingService.CreateIndeterminateLoader();
            loader.Hidden = true;

            View.AddSubview(loader);
            loadingService.LayoutAtTopOfViewController(loader, this);

            this.NavigationItem.TitleView = new UIImageView(UIImage.FromFile("AppLogo/AppLogoForNav.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate));
            this.NavigationItem.TitleView.TintColor = UIColor.White;

            ticketsListView.RegisterNibForCellReuse(UINib.FromName("MyTicketsTableViewCell", null), "MyTicketsTableViewCell");
            ticketsListView.RegisterNibForCellReuse(UINib.FromName("MyTicketsTitleTableViewCell", null), "MyTicketsTitleTableViewCell");

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };
        }

        void ShowNextTicketView()
        {
            var view = new NextTicketViewController();
            view.Model = nextTicket;

            view.View.Frame = new CoreGraphics.CGRect(x: 0, y: 0, width: ticketsListView.Frame.Width, height: 537);
            ticketsListView.TableHeaderView = view.View;

            ticketStatusViewController = view;
        }

        void ShowNoTicketView()
        {
            var view = new NoTicketViewController();
            view.View.Frame = new CoreGraphics.CGRect(x: 0, y: 0, width: ticketsListView.Frame.Width, height: 136);
            ticketsListView.TableHeaderView = view.View;

            ticketStatusViewController = view;
        }

        void ShowLoader(bool visible)
        {
            loader.Hidden = !visible;

            if (visible)
                loader.StartAnimating();
            else
                loader.StopAnimating();
        }

        async Task LoadTickets()
        {
            var currentAccount = loginService.CurrentAccount();

            ShowLoader(true);

            // the MADE api currently returns ALL the tickets for both
            // HWB and LAPHIL. Our apps though only know about
            // events for thier respective purpose. AKA HWB only knows
            // about HWB events. So we need to filter out tickets
            // that might be for the <other> venue.
            var tickets = await ticketsService.GetTickets(currentAccount);
            var events = await eventService.CurrentEvents();
            var eventIds = new HashSet<string>(events.Select(x => x.TicketingSystemId));
            var allowedTickets = tickets.Where(x => eventIds.Contains(x.PerformanceId)).ToArray();

            ShowLoader(false);

            GetMyTickets(allowedTickets);
            GetEventList(events);
        }

        void LoadEvents()
        {
            eventService.CurrentEvents().Subscribe((Event[] obj) => GetEventList(obj));
        }

        private Task<bool> ShouldReloadEvents(Event[] obj)
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

        private void GetEventList(Event[] obj)
        {
            var nextTicketEvent = obj.Where(x => x.TicketingSystemId == nextTicket.PerformanceId).FirstOrDefault();

            var nextTicketViewController = ticketStatusViewController as NextTicketViewController;
            if (nextTicketViewController != null)
                nextTicketViewController.Event = nextTicketEvent;

            eventList = obj;
        }

        private void GetMyTickets(Ticket[] obj)
        {
            if ((obj == null) || (obj.Length == 0))
            {
                ShowNoTicketView();
            }
            else
            {                 var candidateTicket = obj[0]; 

                if(nextTicket == null || nextTicket.PerformanceId != candidateTicket.PerformanceId)
                {
                    nextTicket = candidateTicket;
                    myCurrentTickets = obj;

                    ShowNextTicketView();
                    LoadEvents();
                }

                RegisterNextTicketDisposables();
            }

            if (obj.Length > 1)
            {
                ticketsListView.Source = new DataSource(this, obj.Skip(1).ToArray());
                ticketsListView.ReloadData();
            }

        } 
        //internal void showTicketsDetails(Ticket ticket)
        internal void showTicketsDetails(Ticket ticket, int selectedIndex)
        {
            if ((ticket == null) || (eventList == null) || (eventList.Count() == 0))
            {
                var okAlertController = UIAlertController.Create("Oops", "Ticket Currently Unavailable.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return;
            }

            var selectedEvent = eventList.Where(x => x.TicketingSystemId == ticket.PerformanceId).FirstOrDefault();

            var storyboard = UIStoryboard.FromName("Main", null);
            var myticketDetailViewController = storyboard.InstantiateViewController("myTicketDetailViewController") as myTicketDetailViewController;

            myticketDetailViewController.myTickets = ticket;

            if (selectedEvent == null)
            {
                Console.WriteLine("EventList Length {0}[{1}]", eventList.Length, selectedIndex);
                if (eventList == null && selectedIndex < eventList.Length && eventList[selectedIndex] == null) {
                    var okAlertController = UIAlertController.Create("Oops", "Ticket Currently Unavailable.", UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(okAlertController, true, null);
                    return;
                }
                selectedEvent = eventList[selectedIndex];
                myticketDetailViewController.SelectedEvent = selectedEvent;
                NavigationController.PushViewController(myticketDetailViewController, true);
            } else {
                myticketDetailViewController.SelectedEvent = selectedEvent;
                NavigationController.PushViewController(myticketDetailViewController, true);
            }
        }


        public void ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);

            var selectedEvent = eventList[index];

            var storyboard = UIStoryboard.FromName("Main", null);
            var programNotesViewController = storyboard.InstantiateViewController("ProgramViewController") as ProgramViewController;
                
            programNotesViewController.Event = selectedEvent;
            NavigationController.PushViewController(programNotesViewController, true);
        }

        void RegisterNextTicketDisposables()
        {
            var nextTicketViewController = ticketStatusViewController as NextTicketViewController;
            
            if (nextTicketViewController != null)
            {
                DisposeBag.Add(nextTicketViewController.Rx.ClickViewTickets.Subscribe(_ => NextTicketView_Click()));
                DisposeBag.Add(nextTicketViewController.Rx.ClickLearn.Subscribe(_ => NextTicketLearn_Click()));
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            DisposeBag = new CompositeDisposable();

            _ = LoadTickets();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (DisposeBag != null)
                DisposeBag.Dispose();
        }

        void NextTicketView_Click()
        {
            showTicketsDetails(nextTicket, 0);
        }

        void NextTicketLearn_Click()
        {
            var target = eventList.Where(x => x.TicketingSystemId == nextTicket.PerformanceId).FirstOrDefault();
            showEventDetails(target);
        }

        void showEventDetails(Event target)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var programNotesViewController = storyboard.InstantiateViewController("ProgramViewController") as ProgramViewController;
            programNotesViewController.Event = target;
            NavigationController.PushViewController(programNotesViewController, true);
        }

        class DataSource : UITableViewSource
        {
            Ticket[] ticketList;
            TimeService timeService = ServiceContainer.Resolve<TimeService>();

            static readonly NSString CellIdentifier = new NSString("MyTicketsTableViewCell");
            private MyTicketViewController _MyTicketViewController;
            public DataSource(UIViewController parentViewController, Ticket[] tmpTicketList)
            {
                _MyTicketViewController = parentViewController as MyTicketViewController;
                ticketList = tmpTicketList;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return ticketList.Length + 1;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Row == 0)
                {                     MyTicketsTitleTableViewCell cell = tableView.DequeueReusableCell("MyTicketsTitleTableViewCell", indexPath) as MyTicketsTitleTableViewCell;                     return cell;                 }
                else
                {                     MyTicketsTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as MyTicketsTableViewCell;
                    var ticketData = ticketList[indexPath.Row - 1] as Ticket;
                    cell.lblProgramName.Text = ticketData.Venue + " " + ticketData.SectionDescription;
                    // cell.lblProgramName.Text = ticketData.Name;
                    if (_MyTicketViewController.eventList != null) {
                        var selectedEvent = _MyTicketViewController.eventList.Where(x => x.TicketingSystemId == ticketData.PerformanceId).FirstOrDefault();
                        if (selectedEvent != null) {
                            cell.lblProgramName.Text = selectedEvent.Program.Name;    
                        }
                    }

                    cell.lblProgramName.AttributedText = ($"<b>{cell.lblProgramName.Text}</b>")
                        .HtmlAttributedString(matchingLabel: cell.lblProgramName);

                    var localTime = ticketData.Date.ToLocalTime();
                    var date1 = localTime.ToString($"ddd / MMM dd, yyyy / {timeService.TimeFormat}");

                    cell.lblDateTime.Text = date1;
                    return cell;
                }
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);

                var ticketData = ticketList[indexPath.Row - 1] as Ticket;
                _MyTicketViewController.showTicketsDetails(ticketData,indexPath.Row);

                tableView.DeselectRow(indexPath, true);
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)             {                 if (indexPath.Row == 0)
                {                     return 65;                 }                 else
                {                     return 100;                 }             }
        }

    }
}
