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
    [Register ("ChooseEventTypeTableCell")]
    partial class ChooseEventTypeTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEventTypeName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewBackground { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblEventTypeName != null) {
                lblEventTypeName.Dispose ();
                lblEventTypeName = null;
            }

            if (viewBackground != null) {
                viewBackground.Dispose ();
                viewBackground = null;
            }
        }
    }
}