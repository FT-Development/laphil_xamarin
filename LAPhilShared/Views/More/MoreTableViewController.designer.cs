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
    [Register ("MoreTableViewController")]
    partial class MoreTableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView moreListview { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (moreListview != null) {
                moreListview.Dispose ();
                moreListview = null;
            }
        }
    }
}