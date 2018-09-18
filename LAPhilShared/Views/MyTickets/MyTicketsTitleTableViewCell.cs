using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS.TableViewCell
{
    public partial class MyTicketsTitleTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MyTicketsTitleTableViewCell");
        public static readonly UINib Nib;

        static MyTicketsTitleTableViewCell()
        {
            Nib = UINib.FromName("MyTicketsTitleTableViewCell", NSBundle.MainBundle);
        }

        protected MyTicketsTitleTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
