using Foundation;
using System;
using UIKit;
using C1.iOS.Calendar;

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
        public UIKit.UUButton btnSubmit { get; set; }

		public CustomSelectionController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Calendar.BackgroundColor = UIColor.Clear;

            Calendar.TextColor = UIColor.White;
            Calendar.SelectionBackgroundColor = UIColor.FromRGB(26, 150, 212);

            Calendar.DisabledTextColor = UIColor.Red;

            //Calendar.Font = UIFont.FromName("Arial", 16);
            //Calendar.TodayFont = UIFont.FromDescriptor(UIFont.FromName("Courier New", 16).FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold), 16);
            //Calendar.TodayTextColor = UIColor.Purple;

            Calendar.DayOfWeekFont = UIFont.FromName("Arial", 12);
            Calendar.HeaderFont = UIFont.FromName("Arial New", 21);

            Calendar.Orientation = CalendarOrientation.Vertical;
            Calendar.CalendarType = CalendarType.Default;

            Calendar.Orientation = CalendarOrientation.Vertical;
            Calendar.DayOfWeekBackgroundColor = UIColor.Clear;

            Calendar.TodayBackgroundColor = UIColor.Gray;

            Calendar.DayBorderWidth = 4.00;
            Calendar.DayBorderColor = UIColor.Clear;

            Calendar.SelectionChanging += OnSelectionChanging;
            Calendar.AdjacentDayTextColor = UIColor.Gray;

            this.viewSubmit.Hidden = true;
            this.imgArrow.Hidden = true;
            this.lblDateStart.TextColor = UIColor.Black;//FromRGB(93, 94, 96);
            this.lblDateEnd.TextColor = UIColor.Black;//FromRGB(93, 94, 96);



        }
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            Calendar.SelectionChanging -= OnSelectionChanging;
        }

        private void OnSelectionChanging(object sender, CalendarSelectionChangingEventArgs e)
        {
            if(e.SelectedDates.Count == 1)
            {
                Console.WriteLine("Stard date : {0} ---- ", e.SelectedDates[0].Date);
                this.lblDateStart.Text = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                this.lblDateEnd.Text = "Choose end date";
                this.lblDateStart.TextColor = UIColor.White;
                this.lblDateEnd.TextColor = UIColor.Black;

                this.viewDate.BackgroundColor = UIColor.FromRGB(93, 94, 96);
                this.viewSubmit.Hidden = true;
                this.imgArrow.Hidden = true;
            }else if (e.SelectedDates.Count > 1)
            {
                var lastSelectedDateIndex = (e.SelectedDates.Count - 1);
                Console.WriteLine("Stard date : {0} ---- End Date --- {1}", e.SelectedDates[0].Date, e.SelectedDates[lastSelectedDateIndex].Date);

                this.lblDateStart.Text = e.SelectedDates[0].Date.ToString("MMM dd, yyyy");
                this.lblDateEnd.Text = e.SelectedDates[lastSelectedDateIndex].Date.ToString("MMM dd, yyyy");
                this.lblDateEnd.TextColor = UIColor.White;
                this.viewDate.BackgroundColor = UIColor.FromRGB(26, 150, 212);
                this.viewSubmit.Hidden = false;
                this.imgArrow.Hidden = false;
            }else
            {
                this.viewDate.BackgroundColor = UIColor.FromRGB(93, 94, 96);
                this.viewSubmit.Hidden = true;
                this.imgArrow.Hidden = true;
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