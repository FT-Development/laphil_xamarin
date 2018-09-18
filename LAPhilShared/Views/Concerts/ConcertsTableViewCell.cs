using System;
using Foundation;
using UIKit;
using LAPhil.Platform;


namespace LAPhil.iOS
{
    public partial class ConcertsTableViewCell : UITableViewCell
    {
        [Outlet]
        public UIButton btnBuyNow { get; set; }

        [Outlet]
        public UIButton btnOpenDetail { get; set; }

        [Outlet]
        public UIButton btnSeeDetails { get; set; }

        [Outlet]
        public UIImageView imgConcert { get; set; }

        [Outlet]
        public UIStackView Accessories { get; set; }

        [Outlet]
        public UILabel AccessoryMessageLabel { get; set; }

        [Outlet]
        public UIView AccessoryBadgeView  { get; set; }

        [Outlet]
        public UILabel AccessoryBadgeLabel { get; set; }

        [Outlet]
        public UILabel lblDate1 { get; set; }

        [Outlet]
        public UILabel lblDate2 { get; set; }

        [Outlet]
        public UILabel lblTitle { get; set; }

        public static readonly NSString Key = new NSString("ConcertsTableViewCell");
        public static readonly UINib Nib;
        public UIImage defaultImage;

        static ConcertsTableViewCell()
        {
            Nib = UINib.FromName("ConcertsTableViewCell", NSBundle.MainBundle);
        }

        protected ConcertsTableViewCell(IntPtr handle) : base(handle)
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
