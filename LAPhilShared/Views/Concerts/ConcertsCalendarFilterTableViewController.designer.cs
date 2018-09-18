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
    [Register ("ConcertsCalendarFilterTableViewController")]
    partial class ConcertsCalendarFilterTableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UITableView filterListview { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (filterListview != null) {
                filterListview.Dispose ();
                filterListview = null;
            }
        }
    }
}