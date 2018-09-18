// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LAPhil.iOS
{
    [Register ("ConcertEventViewController")]
    partial class ConcertEventViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSeeCalendar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ConcertsListVieww { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView loadingIndeicatorConcerts { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewSeeAllEvents { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSeeCalendar != null) {
                btnSeeCalendar.Dispose ();
                btnSeeCalendar = null;
            }

            if (ConcertsListVieww != null) {
                ConcertsListVieww.Dispose ();
                ConcertsListVieww = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (loadingIndeicatorConcerts != null) {
                loadingIndeicatorConcerts.Dispose ();
                loadingIndeicatorConcerts = null;
            }

            if (viewSeeAllEvents != null) {
                viewSeeAllEvents.Dispose ();
                viewSeeAllEvents = null;
            }
        }
    }
}