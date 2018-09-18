using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS
{
    public partial class MoreTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MoreTableViewCell");
        public static readonly UINib Nib;

        [Outlet]
        public UILabel lblText { get; set; }

        static MoreTableViewCell()
        {
            Nib = UINib.FromName("MoreTableViewCell", NSBundle.MainBundle);
        }

        protected MoreTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
