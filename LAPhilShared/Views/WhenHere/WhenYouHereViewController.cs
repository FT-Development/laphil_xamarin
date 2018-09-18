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
    public partial class WhenYouHereViewController : UIViewController
    {
        List<string> itemData;

        [Outlet]
        public UITableView tableListView { get; set; }

        public WhenYouHereViewController (IntPtr handle) : base (handle)
        {
        }

        /*
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var navigationAction = segue.DestinationViewController as NavigationActionButton;
            if(navigationAction != null)
            {
                if (segue.Identifier == "NearbyDining")
                {
                    navigationAction.Label = "NEARBY DINING";
                    navigationAction.Rx.Click.Subscribe(NearbyDining_OnClick);
                }

                if (segue.Identifier == "SeatingChart")
                {
                    navigationAction.Label = "SEATING CHART";
                    navigationAction.Rx.Click.Subscribe(SeatingChart_OnClick);
                }

                if (segue.Identifier == "Accessibility")
                {
                    navigationAction.Label = "ACCESSIBILITY";
                    navigationAction.Rx.Click.Subscribe(Accessibility_OnClick);
                }
            
        }

        public void NearbyDining_OnClick(EventPattern<object> e)
        {
            Console.WriteLine("NEARBY DINING");
        }

        public void SeatingChart_OnClick(EventPattern<object> e)
        {
            Console.WriteLine("SEATING CHART");
        }

        public void Accessibility_OnClick(EventPattern<object> e)
        {
            Console.WriteLine("ACCESSIBILITY");
        }
        */
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.TitleView = new UIImageView(UIImage.FromFile("AppLogo/AppLogoForNav.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate));
            this.NavigationItem.TitleView.TintColor = UIColor.White;

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };


            itemData = LAPhilShared.SharedClass.GetWhereYourHereData();
            tableListView.RegisterNibForCellReuse(UINib.FromName("MoreTableViewCell", null), "MoreTableViewCell");
            tableListView.RegisterNibForCellReuse(UINib.FromName("WhenYouTableViewCell", null), "WhenYouTableViewCell");
            tableListView.Source = new DataSource(this, itemData);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);
            if(index == 0)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                //var nearbyDiningViewController = storyboard.InstantiateViewController("NearbyDiningViewController") as NearbyDiningViewController;
                //NavigationController.PushViewController(nearbyDiningViewController, true);
                var webNearbyDiningViewController = storyboard.InstantiateViewController("WebNearbyDiningViewController") as WebNearbyDiningViewController;
                NavigationController.PushViewController(webNearbyDiningViewController, true);

            }else if(index == 1)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                //var chartViewController = storyboard.InstantiateViewController("ChartViewController") as ChartViewController;
                //NavigationController.PushViewController(chartViewController, true);
                var webChartViewController = storyboard.InstantiateViewController("WebChartViewController") as WebChartViewController;
                NavigationController.PushViewController(webChartViewController, true);

            }
            else if (index == 2)
            {
               var storyboard = UIStoryboard.FromName("Main", null);
               var webAccessibilityViewController = storyboard.InstantiateViewController("WebAccessibilityViewController") as WebAccessibilityViewController;
               NavigationController.PushViewController(webAccessibilityViewController, true);
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
            static readonly NSString CellIdentifier = new NSString("MoreTableViewCell");
            static readonly NSString CellIdentifierWhen = new NSString("WhenYouTableViewCell");

            nfloat hightDisctiontion = 100;


            private WhenYouHereViewController _WhenYouHereViewController;
            List<string> tabledata;

            public DataSource(UIViewController parentViewController, List<string> items)
            {
                _WhenYouHereViewController = parentViewController as WhenYouHereViewController;
                tabledata = items;
            }

            // Customize the number of sections in the table view.
            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return tabledata.Count + 1;
            }
            // Customize the appearance of table view cells.
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if(tabledata.Count == indexPath.Row)
                {
                    WhenYouTableViewCell cellWhen = tableView.DequeueReusableCell(CellIdentifierWhen, indexPath) as WhenYouTableViewCell;
                    //cell.T.Text = tabledata[indexPath.Row].ToString();
                    cellWhen.lblTitle.Text = "Walt Disney Concert Hall";
                    //cellWhen.lblDiscription.Text = "";

                    var disctiptionText = cellWhen.lblDiscription;
                    nfloat width = UIScreen.MainScreen.Bounds.Width - 20;
                    CoreGraphics.CGSize size = ((NSString)disctiptionText.Text).StringSize(disctiptionText.Font, constrainedToSize: new CoreGraphics.CGSize(width, 5000), lineBreakMode: UILineBreakMode.WordWrap);
                    var labelFrame = disctiptionText.Frame;
                    labelFrame.Size = new CoreGraphics.CGSize(width, size.Height);
                    disctiptionText.Lines = int.Parse((disctiptionText.Text.Length / 40).ToString()) + 5000;
                    disctiptionText.Frame = new CoreGraphics.CGRect(10, 70, size.Width, size.Height);
                    this.hightDisctiontion = size.Height;
                    Console.WriteLine("Cell Hight : {0}", this.hightDisctiontion);

                    return cellWhen;
                }else
                {
                    MoreTableViewCell cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as MoreTableViewCell;
                    cell.lblText.Text = tabledata[indexPath.Row].ToString();
                    return cell;
                }

            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("Selected indexPath Row {0}", indexPath.Row);
                tableView.DeselectRow(indexPath, true);
                _WhenYouHereViewController.ViewControllerAtIndex(indexPath.Row);

            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                if (tabledata.Count == indexPath.Row)
                {
                    return hightDisctiontion + 100;
                }
                else
                {
                    return 65;
                }
            }

        }
    }
}