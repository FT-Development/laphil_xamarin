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
using LAPhil.Platform;
using Xamarin.Forms;

namespace LAPhil.iOS
{
    public partial class SupportTableViewController : UITableViewController
    {
        public SupportTableViewController(IntPtr handle) : base(handle)
        {
        }

        List<MoreAction> itemData;
        LoginService loginService = LAPhil.Application.ServiceContainer.Resolve<LoginService>();
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();


        List<MoreAction> SupportInActions()
        {
            return new List<MoreAction>
            {
                new MoreAction{ Label = "CENTENNIAL CAMPAIGN", Action = OnCentennial},
                new MoreAction{ Label = "MAKE A GIFT", Action = OnMakeGift},
                new MoreAction{ Label = "CORPORATE SPONSORSHIP", Action = OnCorporateSponsorship},
                new MoreAction{ Label = "OTHER WAYS TO SUPPORT", Action = OnOtherWaySupport},
            };
        }

        void OnCentennial()
        {
            Console.WriteLine("OnCentennial");
            var vc = (WebSupportUsViewController)Storyboard.InstantiateViewController("WebSupportUsViewController");
            vc.pageTitle = "Centennial Campaign";
            vc.urlsAccessibility = urlService.WebCentennial;
            NavigationController.PushViewController(vc, true);
        }

        void OnMakeGift()
        {
            Console.WriteLine("OnMakeGift");
            var vc = (WebSupportUsViewController)Storyboard.InstantiateViewController("WebSupportUsViewController");
            vc.pageTitle = "Make a Gift";
            vc.urlsAccessibility = urlService.WebMakeAGift;
            NavigationController.PushViewController(vc, true);

        }

        void OnCorporateSponsorship()
        {
            Console.WriteLine("OnCorporateSponsorship");
            var vc = (WebSupportUsViewController)Storyboard.InstantiateViewController("WebSupportUsViewController");
            vc.pageTitle = "Corporate Sponsorship";
            vc.urlsAccessibility = urlService.WebCorporateSponsorship;
            NavigationController.PushViewController(vc, true);

        }

        void OnOtherWaySupport()
        {
            Console.WriteLine("OnOtherWaySupport");
            var vc = (WebSupportUsViewController)Storyboard.InstantiateViewController("WebSupportUsViewController");
            vc.pageTitle = "Other Ways To Support";
            vc.urlsAccessibility = urlService.WebOtherWaysToSupport;
            NavigationController.PushViewController(vc, true);

        }



        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.ConfigureDefaultBackButton();
        }

        public void ConfigureItems()
        {
            itemData = SupportInActions();
            supportListview.RegisterNibForCellReuse(UINib.FromName("MoreTableViewCell", null), "MoreTableViewCell");
            supportListview.Source = new DataSource(this, itemData);
            supportListview.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ConfigureItems();
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
                MoreTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as MoreTableViewCell;
                cell.lblText.Text = tabledata[indexPath.Row].Label;
                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);
                tableView.DeselectRow(indexPath, true);
                var model = tabledata[indexPath.Row];
                model.Action();
            }
        }
    }
}