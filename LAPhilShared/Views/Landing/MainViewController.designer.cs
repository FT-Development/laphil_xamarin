// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using HorizontalSwipe;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LAPhil.iOS
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNext { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNotificationNo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNotificationOk { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNotificationSettingMsgOk { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgAppLogoWithTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgAppLogoWithTitleH { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewAlert { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewNotification { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewNotificationSettingMsg { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnNext != null) {
                btnNext.Dispose ();
                btnNext = null;
            }

            if (btnNotificationNo != null) {
                btnNotificationNo.Dispose ();
                btnNotificationNo = null;
            }

            if (btnNotificationOk != null) {
                btnNotificationOk.Dispose ();
                btnNotificationOk = null;
            }

            if (btnNotificationSettingMsgOk != null) {
                btnNotificationSettingMsgOk.Dispose ();
                btnNotificationSettingMsgOk = null;
            }

            if (imgAppLogoWithTitle != null) {
                imgAppLogoWithTitle.Dispose ();
                imgAppLogoWithTitle = null;
            }

            if (imgAppLogoWithTitleH != null) {
                imgAppLogoWithTitleH.Dispose ();
                imgAppLogoWithTitleH = null;
            }

            if (imgTitle != null) {
                imgTitle.Dispose ();
                imgTitle = null;
            }

            if (viewAlert != null) {
                viewAlert.Dispose ();
                viewAlert = null;
            }

            if (viewNotification != null) {
                viewNotification.Dispose ();
                viewNotification = null;
            }

            if (viewNotificationSettingMsg != null) {
                viewNotificationSettingMsg.Dispose ();
                viewNotificationSettingMsg = null;
            }
        }
    }
}