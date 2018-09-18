using System;

using UIKit;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using ImageIO;
using System.Linq;
using VideoToolbox;
using CoreGraphics;
using HorizontalSwipe;
using Foundation;
using CoreAnimation;
//#if __HB__
using UrbanAirship;
//#endif

using CoreLocation;
using System.Threading.Tasks;

namespace LAPhil.iOS
{
    public partial class MainViewController : UIViewController
    {
        private UIPageViewController pageViewController;
        private List<string> _pageTitles;
        private List<string> _images, _imagesLogo;
        private int currentPageIndex = 0;
        private Boolean isNotificaitonAlertDisplayed = false;
        private HorizontalSwipePageControl _pageControl;

        private Boolean isAppOpen = false;

        private GimbalManager gimbalManager;

        NSObject observer;

        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (NSUserDefaults.StandardUserDefaults.ValueForKey(new NSString("isAppOpen")) != null)
            {                 var vc = this.Storyboard.InstantiateViewController("tabBarRootViewController");                 this.NavigationController.PushViewController(vc, false);             }
            NSUserDefaults.StandardUserDefaults.SetBool(this.isAppOpen, "isAppOpen");             NSUserDefaults.StandardUserDefaults.Synchronize(); 
            Console.WriteLine("this.NibBundle.BundleIdentifier  : {0}", this.NibBundle.BundleIdentifier);
            Console.WriteLine("this.NibBundle.BundlePath  : {0}", this.NibBundle.BundlePath);
            Console.WriteLine("this.NibBundle.AppStoreReceiptUrl  : {0}", this.NibBundle.AppStoreReceiptUrl);

            this.imgTitle.ContentMode = UIViewContentMode.ScaleAspectFit;
            this.imgAppLogoWithTitle.ContentMode = UIViewContentMode.ScaleAspectFit;
            this.imgAppLogoWithTitleH.ContentMode = UIViewContentMode.ScaleAspectFit;

            if (this.NibBundle.BundleIdentifier.ToLower().Contains("com.test.laphil.laphil") ||
                this.NibBundle.BundleIdentifier.ToLower().Contains("com.laphil.laphil"))
            {
                _pageTitles = new List<string> { "Find upcoming shows", "Access your tickets", "Learn about the music" };
                _images = new List<string> { "OnBoardSlide/OnBoardSlide01", "OnBoardSlide/OnBoardSlide02", "OnBoardSlide/OnBoardSlide03" };
                _imagesLogo = new List<string> { "OnBoardSlide/OnBoardSlideIcon01", "OnBoardSlide/OnBoardSlideIcon02", "OnBoardSlide/OnBoardSlideIcon03" };
                this.imgAppLogoWithTitle.Hidden = false;
                this.imgAppLogoWithTitleH.Hidden = true;
                this.imgTitle.Hidden = false;
            }
            else
            {
                _pageTitles = new List<string> { "FIND UPCOMING SHOWS", "ACCESS YOUR TICKETS", "PLAN YOUR VISIT" };
                _images = new List<string> { "OnBoardSlide/OnBoardSlide01", "OnBoardSlide/OnBoardSlide02", "OnBoardSlide/OnBoardSlide03" };
                _imagesLogo = new List<string> { "OnBoardSlide/OnBoardSlideIcon01", "OnBoardSlide/OnBoardSlideIcon02", "OnBoardSlide/OnBoardSlideIcon03" };
                this.imgAppLogoWithTitle.Hidden = true;
                this.imgAppLogoWithTitleH.Hidden = false;
                this.imgTitle.Hidden = true;
            }

            _pageControl = new HorizontalSwipePageControl
            {
                CurrentPage = 0,
                Pages = _pageTitles.Count
            };
            _pageControl.CurrentPage = 0;
            _pageControl.TintColor = UIColor.Clear;
            _pageControl.CurrentPageIndicatorTintColor = UIColor.Clear;
            _pageControl.BackgroundColor = UIColor.Clear;
            _pageControl.PageIndicatorTintColor = UIColor.Clear;



            pageViewController = this.Storyboard.InstantiateViewController("MyPageViewController") as UIPageViewController;
            pageViewController.DataSource = new PageViewControllerDataSource(this, _pageTitles);

            var startVC = this.ViewControllerAtIndex(0) as ContentViewController;
            var viewControllers = new UIViewController[] { startVC };

            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);
            pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height + 40);
            AddChildViewController(this.pageViewController);
            View.AddSubview(this.pageViewController.View);
            pageViewController.DidMoveToParentViewController(this);
            btnNext.TouchUpInside += nextView;
            View.AddSubview(this.btnNext);
            //View.AddSubview(this.lblAppTitle);
            _pageControl.Frame = new CGRect(0, this.View.Frame.Size.Height - 250, this.View.Frame.Width, 100);
            View.Add(_pageControl);
            View.BringSubviewToFront(_pageControl);
            View.BringSubviewToFront(this.imgAppLogoWithTitle);
            View.BringSubviewToFront(this.imgAppLogoWithTitleH);

            //START ADD notification view
            this.isNotificaitonAlertDisplayed = false;

            View.BringSubviewToFront(this.viewAlert);
            this.viewAlert.Hidden = true;

            View.BringSubviewToFront(this.viewNotificationSettingMsg);
            this.viewNotificationSettingMsg.Hidden = true;

            View.BringSubviewToFront(this.viewNotification);
            btnNotificationOk.TouchUpInside += actionNotificationOk;
            btnNotificationNo.TouchUpInside += actionNotificationNo;
            this.viewNotification.Hidden = true;
            this.viewAlert.Hidden = true;

            //END ADD notification view

            View.BringSubviewToFront(this.imgTitle);

            btnNotificationSettingMsgOk.TouchUpInside += actionGotoRoot;
            btnNotificationSettingMsgOk.TouchUpOutside += actionGotoRoot;

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
                actionGotoRoot(btnNotificationSettingMsgOk, null);
            }
        }

        private void nextView(object sender, EventArgs e)
        {
            Console.WriteLine("current index : {0}", this.currentPageIndex);

            if (this.currentPageIndex >= 2)
            {
                if (this.isNotificaitonAlertDisplayed == true)
                {
                    this.isNotificaitonAlertDisplayed = false;
                    var storyboard = UIStoryboard.FromName("Main", null);
                    var myTabBarRootViewController = storyboard.InstantiateViewController("tabBarRootViewController") as tabBarRootViewController;
                    NavigationController.PushViewController(myTabBarRootViewController, true);

                }
                else
                {
                    this.isNotificaitonAlertDisplayed = true;
                    this.viewNotification.Hidden = false;
                    this.viewAlert.Hidden = false;
                }
            }
            else
            {
                this._pageControl.CurrentPage = (this.currentPageIndex + 1);
                var startVC = this.ViewControllerAtIndex(this.currentPageIndex + 1) as ContentViewController;
                var viewControllers = new UIViewController[] { startVC };
                this.pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);

                if (this.currentPageIndex == 2)
                {
                    this.btnNext.SetTitle("GET STARTED", UIControlState.Normal);
                }
                else
                {
                    this.btnNext.SetTitle("NEXT", UIControlState.Normal);
                }
            }

        }

        private void actionGotoRoot(object sender, EventArgs e)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var myTabBarRootViewController = storyboard.InstantiateViewController("tabBarRootViewController") as tabBarRootViewController;
            NavigationController.PushViewController(myTabBarRootViewController, true);
        }

        private void actionNotificationOk(object sender, EventArgs e)
        {
            this.isNotificaitonAlertDisplayed = true;
            this.viewNotification.Hidden = true;
            this.viewAlert.Hidden = true;

            observer = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"NSUserDefaultsDidChangeNotification", DefaultsChanged);

            //if (!NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(10, 0, 0)) &&
            //UAirship.Push.UserPushNotificationsEnabled)
            //{
            //    UAirship.Push.UserPushNotificationsEnabled = pushEnabledSwitch.On;
            //}
            //else if (pushEnabledSwitch.On)
            //{
            //    UAirship.Push.UserPushNotificationsEnabled = true;
            //}

            //UAirship.Location.LocationUpdatesEnabled = locationEnabledSwitch.On;

            //UAirship.Shared.Analytics.Enabled = analyticsSwitch.On;

            // START APNS
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                                   new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            //#if __HB__
              UAirship.Push().UserPushNotificationsEnabled = true;
            //#endif
            // END APNS

        }



        void DefaultsChanged(NSNotification obj)
        {
            Console.WriteLine(" CALL DefaultsChanged after geeting device token" );
            var storyboard = UIStoryboard.FromName("Main", null);
            var myTabBarRootViewController = storyboard.InstantiateViewController("tabBarRootViewController") as tabBarRootViewController;
            NavigationController.PushViewController(myTabBarRootViewController, true);
            if (observer != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
                observer = null;
            }
        }

        private void actionNotificationNo(object sender, EventArgs e)
        {
            
            this.viewNotification.Hidden = true;

            //START     display APSN message
            View.BringSubviewToFront(this.viewNotificationSettingMsg);
            this.viewNotificationSettingMsg.Hidden = false;
            this.btnNotificationSettingMsgOk.TouchUpInside += actionNotificationSettingMsgOk;
            //START     display APSN message
        }

        private void actionNotificationSettingMsgOk(object sender, EventArgs e)
        {
            this.isNotificaitonAlertDisplayed = true;
            this.viewNotificationSettingMsg.Hidden = true;
            this.viewAlert.Hidden = true;
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} _pageTitles.Count {1}", index, _pageTitles.Count);
            this.currentPageIndex = index;
            if (index == _pageTitles.Count)
            {
                this.btnNext.SetTitle("GET STARTED", UIControlState.Normal);
                return null;
            }
            else if (index == -1)
            {
                this.currentPageIndex = 0;
                this.btnNext.SetTitle("NEXT", UIControlState.Normal);
                return null;
            }
            else
            {
                var vc = this.Storyboard.InstantiateViewController("ContentViewController") as ContentViewController;
                this.btnNext.SetTitle("NEXT", UIControlState.Normal);
                vc.imageFile = _images.ElementAt(index);
                vc.imageLogo = _imagesLogo.ElementAt(index);
                vc.titleText = _pageTitles.ElementAt(index);
                vc.pageIndex = index;
                return vc;
            }
        }


        private class PageViewControllerDataSource : UIPageViewControllerDataSource
        {
            private MainViewController _parentViewController;
            private List<string> _pageTitles;

            public PageViewControllerDataSource(UIViewController parentViewController, List<string> pageTitles)
            {
                _parentViewController = parentViewController as MainViewController;
                _pageTitles = pageTitles;
            }

            public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {

                var vc = referenceViewController as ContentViewController;
                var index = vc.pageIndex;
                Console.WriteLine("<<<<\tGetPreviousViewController index : {0} _pageTitles.Count {1}", index, _pageTitles.Count);
                _parentViewController._pageControl.CurrentPage = index;
                if (index == 0)
                {
                    return _parentViewController.ViewControllerAtIndex(-1);
                }
                else
                {
                    index--;
                    return _parentViewController.ViewControllerAtIndex(index);
                }
            }

            public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {

                var vc = referenceViewController as ContentViewController;
                var index = vc.pageIndex;
                Console.WriteLine(">>>>   GetNextViewController index : {0} _pageTitles.Count {1}", index, _pageTitles.Count);
                    
                
                if (index == _pageTitles.Count)
                {
                    return _parentViewController.ViewControllerAtIndex(index);
                }
                else if (index < _pageTitles.Count)
                {
                    _parentViewController._pageControl.CurrentPage = index;
                    index++;
                    return _parentViewController.ViewControllerAtIndex(index);
                }
                else
                {
                    return _parentViewController.ViewControllerAtIndex(0);
                }
        
            }

            public override nint GetPresentationCount(UIPageViewController pageViewController)
            {
                return _pageTitles.Count;
            }

            public override nint GetPresentationIndex(UIPageViewController pageViewController)
            {
                return 0;
            }
        }
    }
}
