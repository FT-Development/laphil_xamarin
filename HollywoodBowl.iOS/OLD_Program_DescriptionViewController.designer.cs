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
    [Register ("OLD_Program_DescriptionViewController")]
    partial class OLD_Program_DescriptionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView AboutThePerformanceComponents { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBuyNow { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BuyNowView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint collectionViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView Components { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DateAndTimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DateAndTimeView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LAPhil.iOS.UITextViewPatched DescriptionTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DescriptionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ExtraMessageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ExtraMessageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgMain { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView piecesCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PiecesView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProducerNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ProducerNameView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SeriesButton { get; set; }

        [Action ("OnBuyNow")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnBuyNow ();

        [Action ("OnExtraMessage")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnExtraMessage ();

        [Action ("OnLearnMoreAboutTheSeries")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnLearnMoreAboutTheSeries ();

        void ReleaseDesignerOutlets ()
        {
            if (AboutThePerformanceComponents != null) {
                AboutThePerformanceComponents.Dispose ();
                AboutThePerformanceComponents = null;
            }

            if (btnBuyNow != null) {
                btnBuyNow.Dispose ();
                btnBuyNow = null;
            }

            if (BuyNowView != null) {
                BuyNowView.Dispose ();
                BuyNowView = null;
            }

            if (collectionViewHeight != null) {
                collectionViewHeight.Dispose ();
                collectionViewHeight = null;
            }

            if (Components != null) {
                Components.Dispose ();
                Components = null;
            }

            if (DateAndTimeLabel != null) {
                DateAndTimeLabel.Dispose ();
                DateAndTimeLabel = null;
            }

            if (DateAndTimeView != null) {
                DateAndTimeView.Dispose ();
                DateAndTimeView = null;
            }

            if (DescriptionTextView != null) {
                DescriptionTextView.Dispose ();
                DescriptionTextView = null;
            }

            if (DescriptionView != null) {
                DescriptionView.Dispose ();
                DescriptionView = null;
            }

            if (ExtraMessageButton != null) {
                ExtraMessageButton.Dispose ();
                ExtraMessageButton = null;
            }

            if (ExtraMessageView != null) {
                ExtraMessageView.Dispose ();
                ExtraMessageView = null;
            }

            if (imgMain != null) {
                imgMain.Dispose ();
                imgMain = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (piecesCollection != null) {
                piecesCollection.Dispose ();
                piecesCollection = null;
            }

            if (PiecesView != null) {
                PiecesView.Dispose ();
                PiecesView = null;
            }

            if (ProducerNameLabel != null) {
                ProducerNameLabel.Dispose ();
                ProducerNameLabel = null;
            }

            if (ProducerNameView != null) {
                ProducerNameView.Dispose ();
                ProducerNameView = null;
            }

            if (SeriesButton != null) {
                SeriesButton.Dispose ();
                SeriesButton = null;
            }
        }
    }
}