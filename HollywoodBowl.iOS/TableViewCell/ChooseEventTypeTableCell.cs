using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS.TableViewCell
{
    public partial class ChooseEventTypeTableCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ChooseEventTypeTableCell");
        public static readonly UINib Nib;

        static ChooseEventTypeTableCell()
        {
            Nib = UINib.FromName("ChooseEventTypeTableCell", NSBundle.MainBundle);
        }

        protected ChooseEventTypeTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
