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
    [Register ("ConcertsTableViewController")]
    partial class ConcertsTableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSeeCalendar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ConcertsListView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSeeCalendar != null) {
                btnSeeCalendar.Dispose ();
                btnSeeCalendar = null;
            }

            if (ConcertsListView != null) {
                ConcertsListView.Dispose ();
                ConcertsListView = null;
            }
        }
    }
}