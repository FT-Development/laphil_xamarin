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
    [Register ("MyTicketViewController")]
    partial class MyTicketViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ticketsListView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ticketsListView != null) {
                ticketsListView.Dispose ();
                ticketsListView = null;
            }
        }
    }
}