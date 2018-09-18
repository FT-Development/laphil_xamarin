using Foundation;
using System;
using UIKit;
using C1.iOS.Calendar;
using LAPhil.Platform;
using System.Threading.Tasks;

//HELP :  http://help.grapecity.com/componentone/NetHelp/Xamarin-iOS/webframe.html#C1.iOS.Calendar~C1.iOS.Calendar.CalendarDaysOfWeekPanel.html

namespace LAPhil.iOS
{
	public partial class CustomSelectionController : UIViewController
	{
        [Outlet]
        public C1.iOS.Calendar.C1Calendar Calendar { get; set; }

        [Outlet]
        public UIKit.UILabel lblDateEnd { get; set; }

        [Outlet]
        public UIKit.UILabel lblDateStart { get; set; }

        [Outlet]
        public UIKit.UIView viewDate { get; set; }

        [Outlet]
        public UIKit.UIView viewSubmit { get; set; }

        [Outlet]
        public UIKit.UIImageView imgArrow { get; set; }

        [Outlet]
        public UIKit.UIButton btnClearDates { get; set; }

        [Outlet]
        public UIKit.UIButton btnSubmit { get; set; }


        String dateStart;
        String dateEnd;

        bool isFistDateSelected = false;


		public CustomSelectionController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            isFistDateSelected = false;

            this.NavigationController.NavigationBar.Hidden = false;
            this.NavigationController.NavigationBar.BackgroundColor = UIColor.Clear;
            this.NavigationItem.Title = "Choose Your Date(s)";
            this.ConfigureDefaultBackButton();
            this.View.BackgroundColor = UIColor.FromRGB(35, 31, 32);

            this.btnClearDates.TouchUpInside += clearDates;
            this.btnSubmit.TouchUpInside += actionSubmit;

            Calendar.SelectionChanging += OnSelectionChanging;

            clearDates(null, null);

        }
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            Calendar.SelectionChanging -= OnSelectionChanging;
        }

        private void actionSubmit(object sender, EventArgs e)
        {
            if (dateStart != null)
            {
                NSUserDefaults.StandardUserDefaults.SetString(dateStart, "selectedStartDate");
                if (dateEnd != null)
                {
                    NSUserDefaults.StandardUserDefaults.SetString(dateEnd, "selectedEndDate");
                } else {
                    NSUserDefaults.StandardUserDefaults.SetString(dateStart, "selectedEndDate");
                }
            }

            NSUserDefaults.StandardUserDefaults.Synchronize();
            this.NavigationController.PopViewController(true);

            //Clear
            //NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEventType");
            //NSUserDefaults.StandardUserDefaults.RemoveObject("selectedStartDate");
            //NSUserDefaults.StandardUserDefaults.RemoveObject("selectedEndDate");
            //Harish_M31
            //NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"Notification_FilterEventType", null);
            //Task.Delay(500);
            //this.DismissViewController(true, null);
        }

        private void clearDates(object sender, EventArgs e)
        {
            isFistDateSelected = false;
            Calendar.BackgroundColor = UIColor.Clear;

            Calendar.TextColor = UIColor.White;
            Calendar.SelectionBackgroundColor = UIColor.FromRGB(26, 150, 212);

            Calendar.DisabledTextColor = UIColor.Red;

            //Calendar.Font = UIFont.FromName("Arial", 16);
            //Calendar.TodayFont = UIFont.FromDescriptor(UIFont.FromName("Courier New", 16).FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold), 16);
            //Calendar.TodayTextColor = UIColor.Purple;

            //Calendar.DayOfWeekFont = UIFont.FromName("Arial", 12);
            //Calendar.HeaderFont = UIFont.FromName("Arial New", 21);

            Calendar.DayOfWeekFont = UIFont.FromName("ApercuPro-Medium", 12);
            Calendar.HeaderFont = UIFont.FromName("ApercuPro-Bold", 21);

            Calendar.Orientation = CalendarOrientation.Vertical;
            Calendar.CalendarType = CalendarType.Default;

            Calendar.Orientation = CalendarOrientation.Vertical;
            Calendar.DayOfWeekBackgroundColor = UIColor.Clear;

            Calendar.TodayBackgroundColor = UIColor.Gray;

            Calendar.DayBorderWidth = 4.00;
            Calendar.DayBorderColor = UIColor.Clear;

            //Calendar.SelectionChanging += OnSelectionChanging;
            Calendar.AdjacentDayTextColor = UIColor.Gray;

            //this.viewSubmit.Hidden = true;
            this.imgArrow.Hidden = true;
            Calendar.SelectedDate = null;
            Calendar.SelectedDate = null;
            this.dateStart = null;
            this.dateEnd = null;
            this.lblDateStart.Text = "Choose Start Date";
            this.lblDateEnd.Text = "Choose End Date";
            this.lblDateStart.TextColor = UIColor.White;//FromRGB(93, 94, 96);
            this.lblDateEnd.TextColor = UIColor.Black;//FromRGB(93, 94, 96);
            this.viewDate.BackgroundColor = UIColor.FromRGB(93, 94, 96);
        }

        private void OnSelectionChanging(object sender, CalendarSelectionChangingEventArgs e)
        {
            if (e.SelectedDates.Count == 1)
            {
                Console.WriteLine("Stard date : {0} ---- ", e.SelectedDates[0].Date);
                if(isFistDateSelected == false)//(this.lblDateStart.Text == "Choose Start Date")
                {
                    isFistDateSelected = true;

                    this.lblDateStart.Text = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                    this.lblDateEnd.Text = "Choose End Date";
                    this.lblDateStart.TextColor = UIColor.Black;
                    this.lblDateEnd.TextColor = UIColor.White;
                    //this.lblDateStart.TextColor = UIColor.White;
                    //this.lblDateEnd.TextColor = UIColor.Black;

                    this.viewDate.BackgroundColor = UIColor.FromRGB(93, 94, 96);
                    //this.viewSubmit.Hidden = true;
                    this.imgArrow.Hidden = true;

                    this.dateStart = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                    this.dateEnd = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");

                    //Calendar.SelectionChanging -= OnSelectionChanging;
                    //Calendar.SelectionChanging += OnSelectionChanging;

                }else
                {
                    isFistDateSelected = false;
                    //var lastSelectedDateIndex = (e.SelectedDates.Count - 1);
                    Console.WriteLine("End Date --- {0}", e.SelectedDates[0].Date);

                    this.lblDateStart.Text = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                    this.lblDateEnd.Text = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");//e.SelectedDates[lastSelectedDateIndex].Date.ToString("MMM dd, yyyy");
                    //this.lblDateEnd.TextColor = UIColor.White;
                    this.lblDateEnd.TextColor = UIColor.Black;
                    this.viewDate.BackgroundColor = UIColor.FromRGB(26, 150, 212);
                    //this.viewSubmit.Hidden = false;
                    this.imgArrow.Hidden = false;

                    this.dateStart = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                    this.dateEnd = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");//e.SelectedDates[lastSelectedDateIndex].Date.ToString("MMM dd, yyyy");

                    //Calendar.SelectionChanging -= OnSelectionChanging;
                    //Calendar.SelectionChanging += OnSelectionChanging;

                }

            } else if (e.SelectedDates.Count > 1) {
                isFistDateSelected = false;
                var lastSelectedDateIndex = (e.SelectedDates.Count - 1);
                Console.WriteLine("Stard date : {0} ---- End Date --- {1}", e.SelectedDates[0].Date, e.SelectedDates[lastSelectedDateIndex].Date);

                this.lblDateStart.Text = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                this.lblDateEnd.Text = e.SelectedDates[lastSelectedDateIndex].Date.ToString("MMM dd, yyyy");
                //this.lblDateEnd.TextColor = UIColor.White;
                this.lblDateEnd.TextColor = UIColor.Black;
                this.viewDate.BackgroundColor = UIColor.FromRGB(26, 150, 212);
                //this.viewSubmit.Hidden = false;
                this.imgArrow.Hidden = false;

                this.dateStart = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                this.dateEnd   = e.SelectedDates[lastSelectedDateIndex].Date.ToString("MMM dd, yyyy");
            } else {
                isFistDateSelected = false;
                this.viewDate.BackgroundColor = UIColor.FromRGB(93, 94, 96);
                //this.viewSubmit.Hidden = true;
                this.imgArrow.Hidden = true;
                this.lblDateStart.TextColor = UIColor.White;
                this.lblDateEnd.TextColor = UIColor.Black;
            }

            /*
            foreach (var date in e.SelectedDates.ToArray())
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    e.SelectedDates.Remove(date);
            }
            */
        }

    }
}
