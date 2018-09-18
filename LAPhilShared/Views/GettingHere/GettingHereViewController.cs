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
    public partial class GettingHereViewController : UITableViewController
    {
        List<string> itemData;

        public GettingHereViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
            UIImage.FromFile("Others/btnBack.png"), UIBarButtonItemStyle.Plain, (sender, args) => {
                this.NavigationController.PopViewController(true);
            }), true);
            this.NavigationItem.LeftBarButtonItem.TintColor = UIColor.White;

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };

            itemData = LAPhilShared.SharedClass.GetGettingHereData();
            tableListView.RegisterNibForCellReuse(UINib.FromName("MoreTableViewCell", null), "MoreTableViewCell");
            tableListView.RegisterNibForCellReuse(UINib.FromName("GettingHereTableViewCell", null), "GettingHereTableViewCell");
            tableListView.Source = new DataSource(this, itemData);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", index);
            if (index == 0)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var parkingViewController = storyboard.InstantiateViewController("ParkingViewController") as ParkingViewController;
                NavigationController.PushViewController(parkingViewController, true);
            }else if (index == 1)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var WebMetroViewController = storyboard.InstantiateViewController("WebMetroViewController") as WebMetroViewController;
                WebMetroViewController.strTitle = "Metro";
                NavigationController.PushViewController(WebMetroViewController, true);
            }else
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var WebMetroViewController = storyboard.InstantiateViewController("WebMetroViewController") as WebMetroViewController;
                WebMetroViewController.strTitle = "Bus";
                NavigationController.PushViewController(WebMetroViewController, true);
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
            static readonly NSString CellIdentifierWhen = new NSString("GettingHereTableViewCell");

            nfloat hightDisctiontion = 100;


            private GettingHereViewController _GettingHereViewController;
            List<string> tabledata;

            public DataSource(UIViewController parentViewController, List<string> items)
            {
                _GettingHereViewController = parentViewController as GettingHereViewController;
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
                if (tabledata.Count == indexPath.Row)
                {
                    GettingHereTableViewCell cellGetting = tableView.DequeueReusableCell(CellIdentifierWhen, indexPath) as GettingHereTableViewCell;

                    //cellGetting.lblTitle.Text = "";
                    //cellGetting.lblDiscription.Text = "";

                    var disctiptionText = cellGetting.lblDiscription;
                    nfloat width = UIScreen.MainScreen.Bounds.Width - 20;
                    CoreGraphics.CGSize size = ((NSString)disctiptionText.Text).StringSize(disctiptionText.Font, constrainedToSize: new CoreGraphics.CGSize(width, 5000), lineBreakMode: UILineBreakMode.WordWrap);
                    var labelFrame = disctiptionText.Frame;
                    labelFrame.Size = new CoreGraphics.CGSize(width, size.Height);
                    disctiptionText.Lines = int.Parse((disctiptionText.Text.Length / 40).ToString()) + 5000;
                    disctiptionText.Frame = new CoreGraphics.CGRect(10, 70, size.Width, size.Height);
                    this.hightDisctiontion = size.Height;
                    Console.WriteLine("Cell Hight : {0}", this.hightDisctiontion);

                    return cellGetting;
                }
                else
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
                _GettingHereViewController.ViewControllerAtIndex(indexPath.Row);

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