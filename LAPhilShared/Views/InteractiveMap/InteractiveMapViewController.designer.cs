// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LAPhil.iOS
{
    [Register ("InteractiveMapViewController")]
    partial class InteractiveMapViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint imageViewBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint imageViewLeadingConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint imageViewTopConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint imageViewTrailingConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (imageViewBottomConstraint != null) {
                imageViewBottomConstraint.Dispose ();
                imageViewBottomConstraint = null;
            }

            if (imageViewLeadingConstraint != null) {
                imageViewLeadingConstraint.Dispose ();
                imageViewLeadingConstraint = null;
            }

            if (imageViewTopConstraint != null) {
                imageViewTopConstraint.Dispose ();
                imageViewTopConstraint = null;
            }

            if (imageViewTrailingConstraint != null) {
                imageViewTrailingConstraint.Dispose ();
                imageViewTrailingConstraint = null;
            }

            if (scrollView != null) {
                scrollView.Dispose ();
                scrollView = null;
            }
        }
    }
}