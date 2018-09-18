using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS.TableViewCell
{
    public partial class GettingHereTableViewCell : UITableViewCell
    {
        [Outlet]
        public UILabel lblDiscription { get; set; }

        [Outlet]
        public UILabel lblTitle { get; set; }

        public static readonly NSString Key = new NSString("GettingHereTableViewCell");
        public static readonly UINib Nib;

        static GettingHereTableViewCell()
        {
            Nib = UINib.FromName("GettingHereTableViewCell", NSBundle.MainBundle);
        }

        protected GettingHereTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
