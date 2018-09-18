// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LAPhil.iOS.TableViewCell
{
    [Register ("ChooseEventTypesTableCell")]
    partial class ChooseEventTypesTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEventType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewBackground { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblEventType != null) {
                lblEventType.Dispose ();
                lblEventType = null;
            }

            if (viewBackground != null) {
                viewBackground.Dispose ();
                viewBackground = null;
            }
        }
    }
}