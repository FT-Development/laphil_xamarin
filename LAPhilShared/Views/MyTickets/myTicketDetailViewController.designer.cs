// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LAPhil.iOS
{
    [Register ("myTicketDetailViewController")]
    partial class myTicketDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAddAppleWallet { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAddToCalendar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLearn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnVisitOurWebsite { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView TicketCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView viewScrollMyTicket { get; set; }


        [Action ("Learn_Click:")]
        partial void Learn_Click (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnAddAppleWallet != null) {
                btnAddAppleWallet.Dispose ();
                btnAddAppleWallet = null;
            }

            if (btnAddToCalendar != null) {
                btnAddToCalendar.Dispose ();
                btnAddToCalendar = null;
            }

            if (btnLearn != null) {
                btnLearn.Dispose ();
                btnLearn = null;
            }

            if (btnVisitOurWebsite != null) {
                btnVisitOurWebsite.Dispose ();
                btnVisitOurWebsite = null;
            }

            if (TicketCollectionView != null) {
                TicketCollectionView.Dispose ();
                TicketCollectionView = null;
            }

            if (viewScrollMyTicket != null) {
                viewScrollMyTicket.Dispose ();
                viewScrollMyTicket = null;
            }
        }
    }
}