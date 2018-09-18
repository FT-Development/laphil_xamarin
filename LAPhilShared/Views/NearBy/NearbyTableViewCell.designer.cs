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
    [Register ("NearbyTableViewCell")]
    partial class NearbyTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDiscription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPhoneNo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblViewWebSite { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblDiscription != null) {
                lblDiscription.Dispose ();
                lblDiscription = null;
            }

            if (lblPhoneNo != null) {
                lblPhoneNo.Dispose ();
                lblPhoneNo = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (lblViewWebSite != null) {
                lblViewWebSite.Dispose ();
                lblViewWebSite = null;
            }
        }
    }
}