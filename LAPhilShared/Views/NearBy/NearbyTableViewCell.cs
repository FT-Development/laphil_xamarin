using System;

using Foundation;
using UIKit;

namespace LAPhil.iOS.TableViewCell
{
    public partial class NearbyTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("NearbyTableViewCell");
        public static readonly UINib Nib;

        static NearbyTableViewCell()
        {
            Nib = UINib.FromName("NearbyTableViewCell", NSBundle.MainBundle);
        }

        protected NearbyTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
