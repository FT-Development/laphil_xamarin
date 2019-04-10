using UIKit;
using Foundation;
using CoreFoundation;
using System.Runtime.CompilerServices;
using System;
using System.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Threading;
using LAPhil.User;
using LAPhil.Application;
using LAPhil.Events;
using LAPhil.Urls;
using LAPhilShared;
using Xamarin.Forms;
using UrbanAirship;

namespace LAPhil.iOS
{
    public class MoreAction
    {
        public string Label { get; set; }
        public Action Action { get; set; }
    }

    public partial class MoreTableViewController : UITableViewController
    {
        List<MoreAction> itemData;
        LoginService loginService = LAPhil.Application.ServiceContainer.Resolve<LoginService>();
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();


        public MoreTableViewController(IntPtr handle) : base(handle)
        {
        }

        List<MoreAction> LoggedInActions()
        {
            var hbList = new List<MoreAction>
            {
                new MoreAction{ Label = "GETTING HERE", Action = OnGettingHere},
                new MoreAction{ Label = "SEATING CHART", Action = OnSeatingChart},
                new MoreAction{ Label = "MOBILE ORDERING", Action = OnMobileOrderingClick},
                new MoreAction{ Label = "MAP", Action = OnInteractiveMap},
                new MoreAction{ Label = "THE BOWL STORE", Action = OnBowlStoreClick},
                new MoreAction{ Label = "FAQ", Action = OnFAQ},
                new MoreAction{ Label = "ABOUT US", Action = OnAboutUs},
                new MoreAction{ Label = "SUPPORT US", Action = OnSupportUs},
                new MoreAction{ Label = "MY ACCOUNT DETAILS", Action = OnMyAccountDetails},
                new MoreAction{ Label = "INBOX", Action = OnInbox},
                new MoreAction{ Label = "LOG OUT", Action = OnLogout},
                new MoreAction{ Label = "PRIVACY POLICY", Action = OnPrivacyPolicyClick}
            };
            var laList = new List<MoreAction>
            {
                new MoreAction{ Label = "GETTING HERE", Action = OnGettingHere},
                new MoreAction{ Label = "SEATING CHART", Action = OnSeatingChart},
                new MoreAction{ Label = "LA PHIL STORE", Action = OnLAStoreClick},
                new MoreAction{ Label = "FAQ", Action = OnFAQ},
                new MoreAction{ Label = "ABOUT US", Action = OnAboutUs},
                new MoreAction{ Label = "SUPPORT US", Action = OnSupportUs},
                new MoreAction{ Label = "MY ACCOUNT DETAILS", Action = OnMyAccountDetails},
                new MoreAction{ Label = "INBOX", Action = OnInbox},
                new MoreAction{ Label = "LOG OUT", Action = OnLogout},
                new MoreAction{ Label = "PRIVACY POLICY", Action = OnPrivacyPolicyClick}
            };
            #if __HB__
            return hbList;
            #endif
            return laList;
        }

        List<MoreAction> LoggedOutActions()
        {
            var hbList = new List<MoreAction>
            {
                new MoreAction{ Label = "GETTING HERE", Action = OnGettingHere},
                new MoreAction{ Label = "SEATING CHART", Action = OnSeatingChart},
                new MoreAction{ Label = "MOBILE ORDERING", Action = OnMobileOrderingClick},
                new MoreAction{ Label = "MAP", Action = OnInteractiveMap},
                new MoreAction{ Label = "THE BOWL STORE", Action = OnBowlStoreClick},
                new MoreAction{ Label = "FAQ", Action = OnFAQ},
                new MoreAction{ Label = "ABOUT US", Action = OnAboutUs},
                new MoreAction{ Label = "SUPPORT US", Action = OnSupportUs},
                new MoreAction{ Label = "INBOX", Action = OnInbox},
                new MoreAction{ Label = "LOG IN", Action = OnLogin},
                new MoreAction{ Label = "PRIVACY POLICY", Action = OnPrivacyPolicyClick}
            };
            var laList = new List<MoreAction>
            {
                new MoreAction{ Label = "GETTING HERE", Action = OnGettingHere},
                new MoreAction{ Label = "SEATING CHART", Action = OnSeatingChart},
                new MoreAction{ Label = "LA PHIL STORE", Action = OnLAStoreClick},
                new MoreAction{ Label = "FAQ", Action = OnFAQ},
                new MoreAction{ Label = "ABOUT US", Action = OnAboutUs},
                new MoreAction{ Label = "SUPPORT US", Action = OnSupportUs},
                new MoreAction{ Label = "INBOX", Action = OnInbox},
                new MoreAction{ Label = "LOG IN", Action = OnLogin},
                new MoreAction{ Label = "PRIVACY POLICY", Action = OnPrivacyPolicyClick}
            };
            #if __HB__
            return hbList;
            #else
            return laList;
            #endif
        }

        void OnInbox()
        {
            UAirship.Push().UserPushNotificationsEnabled = true;
            var app = UIApplication.SharedApplication.Delegate as AppDelegate;
            app.inboxDelegate.ShowInbox();
        }

        void OnInteractiveMap()
        {
            var vc = (InteractiveMapViewController)Storyboard.InstantiateViewController("InteractiveMapViewController");
            NavigationController.PushViewController(vc, true);
        }

        void OnShareThisApp()
        {
            var title = new NSString("LA Phil by Los Angeles Philharmonic Association");
            var url = new NSUrl(urlString: urlService.AppStore);
            var activityController = new UIActivityViewController(
                activityItems: new NSObject[] { title, url },
                applicationActivities: null
            );

            PresentViewController(activityController, animated: true, completionHandler: null);
        }

        void OnMyAccountDetails()
        {
            Device.OpenUri(new Uri(urlService.WebMyAccountDetails));
        }

        void OnAboutUs()
        {
            var vc = (GenericWebViewController)Storyboard.InstantiateViewController("GenericWebViewController");
            vc.pageTitle = "About Us";
            vc.url = urlService.AboutUs;
            NavigationController.PushViewController(vc, true);
        }

        void OnSeatingChart()
        {
            var vc = (WebSupportUsViewController)Storyboard.InstantiateViewController("WebSupportUsViewController");
            vc.pageTitle = "Seating Chart";
            vc.urlsAccessibility = urlService.WebSeatingChart;
            NavigationController.PushViewController(vc, true);
        }

        void OnFAQ()
        {
            var vc = (WebFaqViewController)Storyboard.InstantiateViewController("WebFaqViewController");
            NavigationController.PushViewController(vc, true);
        }

        void OnSupportUs()
        {
            var vc = (SupportTableViewController)Storyboard.InstantiateViewController("SupportTableViewController");
            NavigationController.PushViewController(vc, true);
        }

        void OnGettingHere()
        {
            var vc = (WebGettingHereViewController)Storyboard.InstantiateViewController("WebGettingHereViewController");
            NavigationController.PushViewController(vc, true);
        }

        void OnLAStoreClick()
        {
            var vc = (GenericWebViewController)Storyboard.InstantiateViewController("GenericWebViewController");
            vc.pageTitle = "LA Phil Store";
            vc.url = urlService.LAStore;
            NavigationController.PushViewController(vc, true);

        }

        void OnBowlStoreClick()
        {
            var vc = (GenericWebViewController)Storyboard.InstantiateViewController("GenericWebViewController");
            vc.pageTitle = "The Bowl Store";
            vc.url = urlService.BowlStore;
            NavigationController.PushViewController(vc, true);

        }

        void OnMobileOrderingClick()
        {
            var vc = (GenericWebViewController)Storyboard.InstantiateViewController("GenericWebViewController");
            vc.pageTitle = "Mobile Ordering";
            vc.url = urlService.MobileOrdering;
            vc.removeHeader = false;
            NavigationController.PushViewController(vc, true);

        }

        void OnPrivacyPolicyClick()
        {
            var vc = (GenericWebViewController)Storyboard.InstantiateViewController("GenericWebViewController");
            vc.pageTitle = "Privacy Policy";
            vc.url = urlService.PrivacyPolicy;
            vc.removeHeader = false;
            vc.removePrivacyAlert = true;
            NavigationController.PushViewController(vc, true);

        }

        void OnLogin()
        {
            _ = ShowLogin();
        }

        void OnLogout()
        {
            //Create Alert
            var okCancelAlertController = UIAlertController.Create("Are you sure?", "Log Out", UIAlertControllerStyle.Alert);
            //Add Actions
            okCancelAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, alert => Console.WriteLine("Cancel was clicked")));
            okCancelAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Cancel, alert => Logout()));
            //Present Alert
            PresentViewController(okCancelAlertController, true, null);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.TitleView = new UIImageView(UIImage.FromFile("AppLogo/AppLogoForNav.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate));
            NavigationItem.TitleView.TintColor = UIColor.White;

            NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };
        }

        public void ConfigureItems()
        {
            Console.WriteLine("loginService.IsLoggedIn() :{0} ",loginService.IsLoggedIn());

            if (loginService.IsLoggedIn())
            {
                itemData = LoggedInActions();
            }
            else
            {
                itemData = LoggedOutActions();
            }

            moreListview.RegisterNibForCellReuse(UINib.FromName("MoreTableViewCell", null), "MoreTableViewCell");
            moreListview.Source = new DataSource(this, itemData);
            moreListview.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ConfigureItems();
        }

        public void Logout()
        {
            NSUserDefaults.StandardUserDefaults.SetString("0", "isLoginOpen");
            var islogin = loginService.Logout();
            ConfigureItems();
        }


        public async Task ShowLogin()
        {
            NSUserDefaults.StandardUserDefaults.SetString("comefromMore", "isLoginOpen");
            var storyboard = UIStoryboard.FromName("Main", null);
            var myloginNavigationView = storyboard.InstantiateViewController("loginNavigationView") as loginNavigationView;

            PresentModalViewController(myloginNavigationView, true);

            var account = await loginService.Rx.Login;

            DismissViewController(animated: true, completionHandler: null);

            if (account == null)
                return;
        }


        class DataSource : UITableViewSource
        {
            static readonly NSString CellIdentifier = new NSString("MoreTableViewCell");

            List<MoreAction> tabledata;

            public DataSource(UIViewController parentViewController, List<MoreAction> items)
            {
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
                MoreTableViewCell cell =  tableView.DequeueReusableCell(CellIdentifier, indexPath) as MoreTableViewCell;
                cell.lblText.Text = tabledata[indexPath.Row].Label;
                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}",indexPath.Row);
                tableView.DeselectRow(indexPath, true);
                var model = tabledata[indexPath.Row];
                model.Action();
            }
        }
    }
}