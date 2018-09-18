using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS
{
    public partial class MyTicketsTableViewCell : UITableViewCell
    {
        [Outlet]
        public UILabel lblDateTime { get; set; }

        [Outlet]
        public UILabel lblProgramName { get; set; }


        public static readonly NSString Key = new NSString("MyTicketsTableViewCell");
        public static readonly UINib Nib;

        static MyTicketsTableViewCell()
        {
            Nib = UINib.FromName("MyTicketsTableViewCell", NSBundle.MainBundle);
        }

        protected MyTicketsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
