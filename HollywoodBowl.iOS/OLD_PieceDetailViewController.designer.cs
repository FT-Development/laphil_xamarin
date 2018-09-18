// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace HollywoodBowl.iOS
{
    [Register ("OLD_PieceDetailViewController")]
    partial class OLD_PieceDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView Components { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ComposerLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView ComposerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LAPhil.iOS.UITextViewPatched DescriptionTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView DescriptionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TitleView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Components != null) {
                Components.Dispose ();
                Components = null;
            }

            if (ComposerLabel != null) {
                ComposerLabel.Dispose ();
                ComposerLabel = null;
            }

            if (ComposerView != null) {
                ComposerView.Dispose ();
                ComposerView = null;
            }

            if (DescriptionTextView != null) {
                DescriptionTextView.Dispose ();
                DescriptionTextView = null;
            }

            if (DescriptionView != null) {
                DescriptionView.Dispose ();
                DescriptionView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (TitleView != null) {
                TitleView.Dispose ();
                TitleView = null;
            }
        }
    }
}