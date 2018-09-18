using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS
{
    public partial class ConcertsCalendarFilterTableViewCell : UITableViewCell
    {
        [Outlet]
        public UILabel lblTitle { get; set; }

        [Outlet]
        public UIImageView imgDetail { get; set; }

        [Outlet]
        public UIKit.UIButton btnDetail { get; set; }

        [Outlet]
        public UIKit.UIView viewBackground { get; set; }


        public static readonly NSString Key = new NSString("ConcertsCalendarFilterTableViewCell");
        public static readonly UINib Nib;

        static ConcertsCalendarFilterTableViewCell()
        {
            Nib = UINib.FromName("ConcertsCalendarFilterTableViewCell", NSBundle.MainBundle);
        }

        protected ConcertsCalendarFilterTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
