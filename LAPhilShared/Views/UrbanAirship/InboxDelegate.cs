using UIKit;
using System;
using UrbanAirship;
using System.Xml.Serialization;
using System.Linq;

namespace LAPhilShared.Views.UrbanAirship
{
    public class InboxDelegate : UAInboxDelegate
    {

        private UIViewController rootViewController;
        private MessageCenterViewController messageCenterViewController;

        public InboxDelegate(UIViewController rootViewController)
        {
            this.rootViewController = rootViewController;
            this.messageCenterViewController = new MessageCenterViewController();
        }

        //MessageCenterViewController MessageCenterViewController()
        //{
        //    UITabBarController tabBarController = (UITabBarController)this.rootViewController;
        //    return (MessageCenterViewController)tabBarController.ViewControllers.ElementAt(2);
        //}

        public override void ShowInboxMessage(UAInboxMessage message) {
            ShowInbox();
            this.messageCenterViewController.DisplayMessage(message);
            //MessageCenterViewController().DisplayMessage(message);
        }

        public override void ShowInbox() 
        {
            //UITabBarController tabBarController = (UITabBarController)this.rootViewController;
            //tabBarController.SelectedIndex = 2;
            //var nav = (UINavigationController)this.rootViewController;
            //nav.PushViewController(this.messageCenterViewController, true);

            //this.rootViewController.PresentViewController(this.messageCenterViewController, true, null);
            this.rootViewController.PresentViewController(this.messageCenterViewController, true, null);
        }
    }
}

