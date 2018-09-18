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
    [Register ("EventTypesTableViewController")]
    partial class EventTypesTableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView eventListview { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnApplyFilter != null) {
                btnApplyFilter.Dispose ();
                btnApplyFilter = null;
            }

            if (eventListview != null) {
                eventListview.Dispose ();
                eventListview = null;
            }
        }
    }
}