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
    [Register ("ContentViewController")]
    partial class ContentViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblScreenTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView OnBoardSlideIcon { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (lblScreenTitle != null) {
                lblScreenTitle.Dispose ();
                lblScreenTitle = null;
            }

            if (OnBoardSlideIcon != null) {
                OnBoardSlideIcon.Dispose ();
                OnBoardSlideIcon = null;
            }
        }
    }
}