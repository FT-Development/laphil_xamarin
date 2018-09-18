using System;
using Foundation;
using UIKit;
using LAPhil.Platform;


namespace LAPhil.iOS
{
    public partial class ConcertsCalendarTableViewCell : UITableViewCell
    {
        [Outlet]
        public UIImageView imgConcert { get; set; }

        [Outlet]
        public UIImageView imgType { get; set; }

        [Outlet]
        public UILabel lblDate1 { get; set; }

        [Outlet]
        public UILabel lblTitle { get; set; }

        [Outlet]
        public UILabel lblHouseRules { get; set; }

        [Outlet]
        public UIStackView Components { get; set; }

        public static readonly NSString Key = new NSString("ConcertsCalendarTableViewCell");
        public static readonly UINib Nib;
        public UIImage defaultImage;


        static ConcertsCalendarTableViewCell()
        {
            Nib = UINib.FromName("ConcertsCalendarTableViewCell", NSBundle.MainBundle);
        }

        protected ConcertsCalendarTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            defaultImage = UIColor.Black.Image(imgConcert.Frame);
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            imgConcert.Image = defaultImage;
            imgConcert.ContentMode = UIViewContentMode.ScaleAspectFill;
            imgConcert.ClipsToBounds = true;
        }
    }
}
