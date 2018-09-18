using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS.TableViewCell
{
    public partial class EventTypesTableViewCell : UITableViewCell
    {

        [Outlet]
        public UIKit.UILabel labelTitle { get; set; }

        [Outlet]
        public UIKit.UIView viewEventTitle { get; set; }

        public static readonly NSString Key = new NSString("EventTypesTableViewCell");
        public static readonly UINib Nib;

        static EventTypesTableViewCell()
        {
            Nib = UINib.FromName("EventTypesTableViewCell", NSBundle.MainBundle);
        }

        protected EventTypesTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
