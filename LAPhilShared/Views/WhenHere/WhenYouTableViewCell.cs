using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS.TableViewCell
{
    public partial class WhenYouTableViewCell : UITableViewCell
    {
        [Outlet]
        public UILabel lblDiscription { get; set; }

        [Outlet]
        public UILabel lblTitle { get; set; }

        public static readonly NSString Key = new NSString("WhenYouTableViewCell");
        public static readonly UINib Nib;

        static WhenYouTableViewCell()
        {
            Nib = UINib.FromName("WhenYouTableViewCell", NSBundle.MainBundle);
        }

        protected WhenYouTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
