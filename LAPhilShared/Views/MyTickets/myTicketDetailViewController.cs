using CoreFoundation;
using CoreGraphics;
using EventKit;
using Foundation;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reactive.Linq;
using UIKit;
using LAPhil.iOS.TableViewCell;
using LAPhil.iOS;
using LAPhil.User;
using LAPhil.Application;
using LAPhil.Events;
using LAPhil.Platform;
using Xamarin.Forms;
using LAPhil.Urls;

using PassKit;
using System.Net;
using System.IO;
using System.Text;


namespace LAPhil.iOS
{
    public partial class myTicketDetailViewController : UIViewController
    {
        public EKEventStore EventStore
        {
            get { return eventStore; }
        }
        protected EKEventStore eventStore = new EKEventStore();

        public Ticket myTickets { get; set; }
        public Event SelectedEvent { get; set; }
        TicketDetail[] myTicketDetail;
        LoginService loginService = ServiceContainer.Resolve<LoginService>();
        TicketsService ticketsService = ServiceContainer.Resolve<TicketsService>();
        LoadingService loadingService = ServiceContainer.Resolve<LoadingService>();
        UIImageView loader;

        LAPhilUrlService urls = ServiceContainer.Resolve<LAPhilUrlService>();
        public myTicketDetailViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            loader = loadingService.CreateIndeterminateLoader();
            loader.Hidden = true;

            View.AddSubview(loader);
            loadingService.LayoutAtTopOfViewController(loader, this);

            //TicketCollectionView.RegisterNibForCell(UINib.FromName("MyNewwCollectionViewCell", null), "MyNewwCollectionViewCell");
            TicketCollectionView.RegisterNibForCell(UINib.FromName("TicketBarcodeCell", null), "TicketBarcodeCell");
            TicketCollectionView.Delegate = new CollectionViewFlowDelegate();

            NavigationItem.Title = "Ticket Detail";
            NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };

            var customLeftButton = new UIBarButtonItem(
            UIImage.FromFile("Others/backConnect.png"),
            
                UIBarButtonItemStyle.Plain,
            (s, e) => {
                NavigationController.PopViewController(true);
            });


            btnVisitOurWebsite.TouchUpInside -= actionOpenVisitOurWebsite;
            btnVisitOurWebsite.TouchUpInside += actionOpenVisitOurWebsite;

            btnLearn.TouchUpInside -= actionLearn;
            btnLearn.TouchUpInside += actionLearn;

            btnAddToCalendar.TouchUpInside -= actionAddToCalendar;
            btnAddToCalendar.TouchUpInside += actionAddToCalendar;

            if (SelectedEvent != null &&
                        SelectedEvent.Description != null &&
                        SelectedEvent.StartTime != null &&
                        SelectedEvent.Program != null &&
                        SelectedEvent.Program.Name != null)
            {
                 btnAddToCalendar.RemoveFromSuperview();
            }

            btnAddAppleWallet.TouchUpInside -= actionAddAppleWallet;
            btnAddAppleWallet.TouchUpInside += actionAddAppleWallet;



            customLeftButton.TintColor = UIColor.White;
            NavigationItem.LeftBarButtonItem = customLeftButton;

            var currentAccount = loginService.CurrentAccount();

            ShowLoader(true);
            ticketsService.GetTicketDetail(currentAccount, myTickets)
                          .Subscribe((TicketDetail[] obj) => GetMyTicketDetail(obj));
        }

        private void actionOpenVisitOurWebsite(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(urls.WebTickets));
        }

        void ShowLoader(bool visible)
        {
            loader.Hidden = !visible;

            if (visible)
                loader.StartAnimating();
            else
                loader.StopAnimating();
        }

        void GetMyTicketDetail(TicketDetail[] obj)
        {
            
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                ShowLoader(false);
            });

            if ((obj == null) || (obj.Length == 0))
            {
                var okAlertController = UIAlertController.Create("Oops", "Ticket Currently Unavailable.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return;
            }
            else
            {
                Console.WriteLine(" Ticket Details Count : {0}", obj.Length);
                myTicketDetail = obj;

                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    TicketCollectionView.Source = new DataSource(this, ticket: myTickets, ticketDetail: myTicketDetail);
                    TicketCollectionView.ReloadData();

                    if (myTicketDetail.Length > 0)
                    {
                        if (myTicketDetail[0].AppleWalletUrl == null)
                        {
                            this.btnAddAppleWallet.Hidden = true;
                        }
                        else
                        {
                            this.btnAddAppleWallet.Hidden = false;
                        }
                    }

                });
            }
        }

        private void actionLearn(object sender, EventArgs e)
        {
            if (SelectedEvent == null) {
                var okAlertController = UIAlertController.Create("Oops", "Ticket Currently Unavailable.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return;
            }
            showEventDetails(SelectedEvent);
        }

        void showEventDetails(Event target)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var programNotesViewController = storyboard.InstantiateViewController("ProgramViewController") as ProgramViewController;
            programNotesViewController.Event = target;
            NavigationController.PushViewController(programNotesViewController, true);
        }

        private void actionAddToCalendar(object sender, EventArgs e)
        {
            Console.WriteLine("actionAddToCalendar");
            eventStore.RequestAccess(EKEntityType.Event,
                (bool granted, NSError error) => {
                var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                var viewController = appDelegate.Window.RootViewController;
                if (granted)
                {
                    Console.WriteLine("actionAddToCalendar access granted");
                    var myEvent = EKEvent.FromStore(eventStore);
                    if (SelectedEvent != null &&
                        SelectedEvent.Description != null &&
                        SelectedEvent.StartTime != null &&
                        SelectedEvent.Program != null &&
                        SelectedEvent.Program.Name != null)
                    {
                        Console.WriteLine("actionAddToCalendar SelectedEvent.Program.Name {0}", SelectedEvent.Program.Name);
                        Console.WriteLine("actionAddToCalendar SelectedEvent.Description {0}", SelectedEvent.Description);
                        Console.WriteLine("actionAddToCalendar SelectedEvent.StartTime {0}", SelectedEvent.StartTime.ToLocalTime());
                        myEvent.Calendar = eventStore.DefaultCalendarForNewEvents;
                        myEvent.Title = SelectedEvent.Program.Name;
                        myEvent.Notes = SelectedEvent.Description;
                        myEvent.StartDate = NSDate.FromTimeIntervalSince1970(SelectedEvent.StartTime.ToLocalTime().ToUnixTimeMilliseconds());
                        myEvent.EndDate = NSDate.FromTimeIntervalSince1970(SelectedEvent.StartTime.ToLocalTime().ToUnixTimeMilliseconds());
                        NSError myEventError = null;
                        eventStore.SaveEvent(myEvent, EKSpan.ThisEvent, out myEventError);
                        if (myEventError != null)
                        {
                            Console.WriteLine("actionAddToCalendar error {0}", myEventError);
                            // new UIAlertView("Calendar Error", myEventError.Description, null, "ok", null).Show();    
                            var okAlertController = UIAlertController.Create("Oops", "Event Currently Unavailable.", UIAlertControllerStyle.Alert);
                            okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                            viewController.PresentViewController(okAlertController, true, null);
                        } else {
                            DispatchQueue.MainQueue.DispatchAsync(() =>
                            {
                                var okAlertController = UIAlertController.Create("Thank you", "Event added to your calendar.", UIAlertControllerStyle.Alert);
                                okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                                viewController.PresentViewController(okAlertController, true, null);
                            });
                        }
                    } else {
                        var okAlertController = UIAlertController.Create("Oops", "Event Currently Unavailable.", UIAlertControllerStyle.Alert);
                        okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                        viewController.PresentViewController(okAlertController, true, null);
                    }
                } else {
                    Console.WriteLine("actionAddToCalendar User denied access to calendar");
                    // new UIAlertView("Access Denied", "User denied access to calendar data", null, "ok", null).Show();
                    var okAlertController = UIAlertController.Create("Oops", "Please enable calendar permissions in your settings.", UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    viewController.PresentViewController(okAlertController, true, null);
                }
            });
        }





        private void actionAddAppleWallet(object sender, EventArgs ea)
        {
            Console.WriteLine("actionAddAppleWallet  :: {0}",myTicketDetail[0].AppleWalletUrl);
            /*  POST API
                https://tickets-dev.laphil.com/api/jwt/performance/tickets
                token:eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1MjE1NDQ5NjksInNlc3Npb24iOiJmYWJhNzljYjJjMTUxMWU4ODBiZDE0MDJlYzMxNmZjMzAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwIn0.lp5gVH4FU_hvBt9WFReA7SvHuGfM0iP2Ef6UQkliN9c
                performance_no:1609
             */

            if (SelectedEvent != null)
            {
                var webClient = new WebClient();
                webClient.DownloadDataCompleted += (s, e) => {
                    var bytes = e.Result;
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string localFilename = "LAPhil001.pkpass";
                    string localPath = Path.Combine(documentsPath, localFilename);

                    Console.WriteLine("localPath:" + localPath);

                    File.WriteAllBytes(localPath, bytes);
                    InvokeOnMainThread(() => {
                        //imageView.Image = UIImage.FromFile(localPath);
                        //new UIAlertView("Done"
                        //, "Image downloaded and saved."
                        //, null
                        //, "OK"
                        //, null).Show();
                    });
                };


                var url = new Uri(myTicketDetail[0].AppleWalletUrl);
                Console.WriteLine("url.AbsoluteUrl : {0}", url.AbsoluteUri);
                webClient.Encoding = Encoding.UTF8;
                webClient.DownloadDataAsync(url);

                InvokeOnMainThread(() => {
                    // HELP : https://docs.microsoft.com/en-us/xamarin/ios/platform/passkit?tabs=vsmac#adding-passes-into-wallet
                    var passHashCode = SelectedEvent.GetHashCode();
                    Console.WriteLine("Apple Wallet passHashCode : " + passHashCode);

                    if (PKPassLibrary.IsAvailable)
                    {

                        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                        var newFilePath = Path.Combine(documentsPath, "LAPhil001.pkpass");
                        //var builtInPassPath = Path.Combine(System.Environment.CurrentDirectory, "LAPhil001.pkpass");
                        //if (!System.IO.File.Exists(newFilePath))
                        //System.IO.File.Copy(builtInPassPath, newFilePath);

                        NSData nsdata;
                        using (FileStream oStream = File.Open(newFilePath, FileMode.Open))
                        {
                            nsdata = NSData.FromStream(oStream);
                        }

                        var err = new NSError(new NSString("42"), -42);
                        var newPass = new PKPass(nsdata, out err);

                        var library = new PKPassLibrary();
                        bool alreadyExists = library.Contains(newPass);

                        if (alreadyExists)
                        {
                            var okAlertController = UIAlertController.Create("LA Phil", "Tickets Already exists.", UIAlertControllerStyle.Alert);
                            //Add Actions
                            okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, alert => {
                            }));
                            PresentViewController(okAlertController, true, null);
                        }
                        else
                        {
                            var pkapvc = new PKAddPassesViewController(newPass);
                            NavigationController.PresentModalViewController(pkapvc, true);
                        }
                    }
                });



            }
        }

        class DataSource : UICollectionViewSource
        {
            //static readonly NSString CellIdentifier = new NSString("MyNewwCollectionViewCell");
            static readonly NSString CellIdentifier = new NSString("TicketBarcodeCell");

            myTicketDetailViewController viewController;
            Ticket ticket;
            TicketDetail[] ticketDetail;
            TicketsService ticketsService = ServiceContainer.Resolve<TicketsService>();

            public DataSource(myTicketDetailViewController myTicketDetailViewController, Ticket ticket, TicketDetail[] ticketDetail)
            {
                viewController = myTicketDetailViewController;
                this.ticketDetail = ticketDetail;
                this.ticket = ticket;
            }

            public override nint NumberOfSections(UICollectionView collectionView)
            {
                return 1;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section)
            {
                return ticketDetail.Length;
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
            {
                //MyNewwCollectionViewCell cell = collectionView.DequeueReusableCell(CellIdentifier, indexPath) as MyNewwCollectionViewCell;
                TicketBarcodeCell cell = collectionView.DequeueReusableCell(CellIdentifier, indexPath) as TicketBarcodeCell;
                var detail = ticketDetail[indexPath.Row];

                cell.eventLabel.Text = $"{ticket.Name} :" + (indexPath.Row + 1);
                cell.ticketCountLabel.Text = (indexPath.Row + 1) +" of "+ ticketDetail.Length + " tickets";

                //cell.labelEventDate.Text = "Thu Oct 19, 2017 at 8:00PM";
                //cell.labelSeatNo.Text = "Orchestra · Row N • Seat " + (indexPath.Row + 1);
                Console.WriteLine("detail.BarcodeUrl {0} {1} {2}", detail.BarcodeUrl, detail.BarcodeUrl == null, detail.BarcodeUrl == "");
                if (detail.BarcodeUrl == null || detail.BarcodeUrl == string.Empty) {
                    DispatchQueue.MainQueue.DispatchAsync(() => {
                        //cell.imageBarcode.Hidden = true;
                       cell.eventLabel.Text = "Ticket Currently Unavailable\n" + $"{ticket.Name} :" + (indexPath.Row + 1);
                    });
                    return cell;
                }

                Task.Run(async () =>
                {
                    var bytes = await ticketsService.GetTicketImageBytes(detail);

                    if (bytes == null)
                    {
                        //DispatchQueue.MainQueue.DispatchAsync(() => {
                        //    cell.imageBarcode.Hidden = true;
                        //});

                        return;
                    }

                    var image = UIImage.LoadFromData(
                        NSData.FromArray(bytes)
                    );

                    DispatchQueue.MainQueue.DispatchAsync(() => {
                        UIView.Transition(
                            withView: cell.imageBarcode,
                            duration: 0.25,
                            options: UIViewAnimationOptions.TransitionCrossDissolve,
                            animation: () => {
                                cell.imageBarcode.Hidden = false;
                                cell.imageBarcode.Image = image;
                            },
                            completion: null);
                    });

                });

                return cell;
            }
        }

        class CollectionViewFlowDelegate : UICollectionViewDelegateFlowLayout
        {
            public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
            {
                return new CGSize(UIScreen.MainScreen.Bounds.Width - 60, 350);
            }
        }
    }
}