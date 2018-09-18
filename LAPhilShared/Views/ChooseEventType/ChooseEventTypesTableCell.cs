using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS.TableViewCell
{
    public partial class ChooseEventTypesTableCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ChooseEventTypesTableCell");
        public static readonly UINib Nib;

        static ChooseEventTypesTableCell()
        {
            Nib = UINib.FromName("ChooseEventTypesTableCell", NSBundle.MainBundle);
        }

        protected ChooseEventTypesTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
