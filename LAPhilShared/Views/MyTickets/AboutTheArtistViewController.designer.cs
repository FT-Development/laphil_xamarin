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
    [Register ("AboutTheArtistViewController")]
    partial class AboutTheArtistViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView programNotesListView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (programNotesListView != null) {
                programNotesListView.Dispose ();
                programNotesListView = null;
            }
        }
    }
}