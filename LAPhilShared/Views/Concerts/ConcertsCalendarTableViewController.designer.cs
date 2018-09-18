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
    [Register ("ConcertsCalendarTableViewController")]
    partial class ConcertsCalendarTableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFilter { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFilterView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ConcertsCalendarListView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewFilter { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewHeader { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFilter != null) {
                btnFilter.Dispose ();
                btnFilter = null;
            }

            if (btnFilterView != null) {
                btnFilterView.Dispose ();
                btnFilterView = null;
            }

            if (ConcertsCalendarListView != null) {
                ConcertsCalendarListView.Dispose ();
                ConcertsCalendarListView = null;
            }

            if (viewFilter != null) {
                viewFilter.Dispose ();
                viewFilter = null;
            }

            if (viewHeader != null) {
                viewHeader.Dispose ();
                viewHeader = null;
            }
        }
    }
}