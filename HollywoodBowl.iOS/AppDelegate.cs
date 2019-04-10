using Foundation;
using UIKit;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.Connectivity;
using LAPhil.HTTP;
using LAPhil.Cache;
using LAPhil.Analytics;
using LAPhil.Analytics.Platform;
using LAPhil.Routing;
using LAPhil.Settings;
using LAPhil.Auth;
using LAPhil.Events;
using LAPhil.User;
using LAPhil.Platform;
using LAPhil.Urls;
using LAPhilShared.Views.UrbanAirship;
using UrbanAirship;
//using CoreLocation;

namespace LAPhil.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate, IUARegistrationDelegate
    {
        ILog Log { get; set; }

        public InboxDelegate inboxDelegate;
        private PushHandler pushHandler;

        public override UIWindow Window
        {
            get;
            set;
        }

        private GimbalManager gimbalManager;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            setupUrbanAirship();
            //setupGimbal();

            Forms.Init();
            ConfigureServices();
            LoadCookies();
            Firebase.Core.App.Configure();
            var container = Google.TagManager.Manager.GetInstance.GetContainer("GTM-KLLCRTG");
                       
            return true;
        }

        //public void setupGimbal()
        //{
        //    //GIMBAL - encapsulate gimbal service management and ios location into following class
        //    gimbalManager = new GimbalManager();
        //    //need an empty dictionary for the overload...just doesn't seem right, does it?
        //    var dict = NSDictionary.FromObjectAndKey((NSString)"nada", (NSString)"nidi");
        //    //sub out your api key here
        //    //GimbalFramework.Gimbal.SetAPIKey("YOUR_API_KEY_HERE", dict);
        //    GimbalFramework.Gimbal.SetAPIKey("b2381352-17c9-4d7d-a7ad-1965da52d507", dict);
        //    //if app was previously authorized, start up gimbal services
        //    if (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways)
        //    {
        //        gimbalManager.Start();
        //    }
        //    //request always auth for location - need a listener since this happens async on start esp on first run
        //    CLLocationManager manager = new CLLocationManager();
        //    manager.Delegate = gimbalManager;
        //    manager.RequestAlwaysAuthorization();
        //}

        public void setupUrbanAirship() 
        {
            // Set log level for debugging config loading (optional)
            // It will be set to the value in the loaded config upon takeOff
            UAirship.SetLogLevel(UALogLevel.Trace);

            // Populate AirshipConfig.plist with your app's info from https://go.urbanairship.com
            // or set runtime properties here.
            UAConfig config = UAConfig.DefaultConfig();
            config.DevelopmentAppKey = "IUNz7rBvRXiNRPstvGxpGg";
            config.DevelopmentAppSecret = "wawJWxFwTOW3n5J5PMUgcg";
            config.ProductionAppKey = "IUNz7rBvRXiNRPstvGxpGg";
            config.ProductionAppSecret = "wawJWxFwTOW3n5J5PMUgcg";

            if (!config.Validate())
            {
                Console.WriteLine("UrbanAirship configurtaion is invalid.");
                return;
            }

            config.MessageCenterStyleConfig = "UAMessageCenterDefaultStyle";

            // Bootstrap the Urban Airship SDK
            UAirship.TakeOff(config);

            Console.WriteLine("UrbanAirship Config:{0}", config);

            UAirship.Push().ResetBadge();

            pushHandler = new PushHandler();
            UAirship.Push().PushNotificationDelegate = pushHandler;

            inboxDelegate = new InboxDelegate(Window.RootViewController);
            UAirship.Inbox().Delegate = inboxDelegate;

            UAirship.Push().WeakRegistrationDelegate = (UrbanAirship.IUARegistrationDelegate) this;

            NSString messageListUpdated = new NSString("com.urbanairship.notification.message_list_updated");

            NSNotificationCenter.DefaultCenter.AddObserver(messageListUpdated, (notification) =>
            {
                refreshMessageCenterBadge();
            });

        }

        private void refreshMessageCenterBadge()
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                if (Window.RootViewController == null)
                {
                    return;
                }

                //UITabBarController tabBarController = (UITabBarController)Window.RootViewController;
                //UITabBarItem messageCenterTab = tabBarController.TabBar.Items[2];

                //if (UAirship.Inbox.MessageList.UnreadCount > 0)
                //{
                //    messageCenterTab.BadgeValue = UAirship.Inbox.MessageList.UnreadCount.ToString();
                //}
                //else
                //{
                //    messageCenterTab.BadgeValue = null;
                //}
            });
        }

        [Export("registrationSucceededForChannelID:deviceToken:")]
        public void RegistrationSucceeded(string channelID, string deviceToken)
        {
            NSNotificationCenter.DefaultCenter.PostNotificationName("channelIDUpdated", this);
        }

        //START       APNS
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Get current device token
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                Console.WriteLine(">>>> DeviceToken {0} --- oldDeviceToken  {1}", DeviceToken, oldDeviceToken);
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"NSUserDefaultsDidChangeNotification", null);

        }
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Console.WriteLine("error.LocalizedDescription >>>> {0}", error.LocalizedDescription);
        }
        //END       APNS

        void ConfigureServices()
        {
            //var serviceURL = "https://tickets-dev.laphil.com";
            //var serviceURL = "http://laphil-staging.herokuapp.com";
            //var serviceURL = "http://tickets-dev.hollywoodbowl.com";
            //var servicesURL = "https://www-dev.hollywoodbowl.com";
            //var servicesURL = "http://tickets-dev.hollywoodbowl.com";
            //var userServicesURL = "https://my.laphil.com";
            //var userServicesURL = "https://my.hollywoodbowl.com";

            // LAPhil URls
            var servicesURL = "https://www.hollywoodbowl.com";
            var userServicesURL = "https://my.hollywoodbowl.com";


            //var servicesURL = "https://www.laphil.com";
            //var userServicesURL = "https://my.laphil.com";

            var cacheStoragePath = PathService.CachePath;
            var settingsStoragePath = PathService.SettingsPath;

            var cacheFilename = Path.Combine(cacheStoragePath, "cache.realm");
            var settingsFilename = Path.Combine(settingsStoragePath, "settings.realm");

            PathService.CreatePath(cacheStoragePath);
            PathService.CreatePath(settingsStoragePath);

            ServiceContainer.Register(new LoggingService(new PlatfromLogger()));
            ServiceContainer.Register(() => new ConnectivityService(new PlatformConnectivityDriver()));
            ServiceContainer.Register(() => new HttpService(timeout: 6.05));

            ServiceContainer.Register(() => new AnalyticsService(new MockAnalyticsDriver()));

	        ServiceContainer.Register(() => new TimeService());
            ServiceContainer.Register(new LoadingService());

            ServiceContainer.Register(() => new LAPhilUrlService(
                webDining: "http://www.hollywoodbowl.com/plan-your-visit/when-youre-here/dining/?mobile_webview=true",
                webChart: "http://www.hollywoodbowl.com/plan-your-visit/when-youre-here/seating-chart/?mobile_webview=true",
                webAccessibility: "http://www.hollywoodbowl.com/plan-your-visit/when-youre-here/accessibility-info/?mobile_webview=true",
                webMetroBus: "http://www.hollywoodbowl.com/plan-your-visit/getting-here/?mobile_webview=true",
                webRegister: "https://my.hollywoodbowl.com/account/register",
                webTickets: "https://my.hollywoodbowl.com/?mobile_webview=true",
                webSupportUs: "http://www.hollywoodbowl.com/support/membership?mobile_webview=true",
                webGettingHere: "http://www.hollywoodbowl.com/plan-your-visit/getting-here/?mobile_webview=true",
                appStore: "https://itunes.apple.com/us/app/hollywood-bowl-app/id1129303947?mt=8",
                webFoodWine: "http://www.hollywoodbowl.com/plan-your-visit/food-wine/?mobile_webview=true",
                faq: "http://www.hollywoodbowl.com/plan-your-visit/faq/?mobile_webview=true",
                centennial: "http://campaign.laphil.com/?mobile_webview=true",
                makeAGift: "https://my.hollywoodbowl.com/support/membership?mobile_webview=true",
                corporateSponsorship: "http://www.hollywoodbowl.com/support-us/corporate-sponsorship/?mobile_webview=true",
                otherWaysToSupport: "http://www.hollywoodbowl.com/support-us/other-ways-support/?mobile_webview=true",
                myAccountDetails: "http://my.hollywoodbowl.com/secure/account/update?mobile_webview=true",
                webSeatingChart: "http://www.hollywoodbowl.com/plan-your-visit/when-youre-here/seating-chart/?mobile_webview=true",
                webForgotPassword: "https://my.hollywoodbowl.com/account/forgottenpassword?mobile_webview=true",
                laStore: "https://www.laphilstore.com",
                bowlStore: "https://www.laphilstore.com/exclusives/hollywood-bowl.html",
                mobileOrdering: "https://hollywoodbowl.splickit.com",
                aboutUs: "https://www.hollywoodbowl.com/about/bowl/",
                privacyPolicy: "https://www.hollywoodbowl.com/privacy-policy/",
                appTitle:"Hollywood Bowl"
            ));

            // Xamarin.Auth does not support NetStardard yet: 1.6 / 1.5.0.4 will.
            // So we have `LAPhil.User.LoginService` is in a Shared Lib.
            // This causes a problem, we need to use it to save our Account 
            // during a Refresh Token operation. So the netstandard lib `LAPhil.User` 
            // declares an interface representing the functionality of 
            // `LAPhil.User.LoginService` which we register above. This 
            // effectively gives the netstandard lib the ability to get it's 
            // hands on the service which it uses in `LAPhil.User.JWTAuth`
            // So below we just fetch the `LoginService` we registered above
            // can cast it to `ILoginService`. There is likely a better
            // way to deal with this that is unknown at this time.
            ServiceContainer.Register(() => new LoginService());
            ServiceContainer.Register(() =>
                (ILoginService)ServiceContainer.Resolve<LoginService>()
            );

            //ServiceContainer.Register(() => new EventService(
            //    new MockEventsDriver()
            //));

            ServiceContainer.Register(() => new EventService(
                new HttpEventsDriver(host: servicesURL)
            ));

            //            ServiceContainer.Register(() => new EventService(
            ////Feb-02    
            //    new HttpEventsDriver(host:serviceURL)
            //    //new HttpEventsDriver(host:"http://laphil-dev.herokuapp.com")
            //));

            //ServiceContainer.Register(() => new AuthService(new MockAuthDriver()));

            ServiceContainer.Register(() => new AuthService(
                new HttpAuthDriver(
                    host: userServicesURL
                //auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));

            //ServiceContainer.Register(() => new UserDigestService(new MockUserDigestDriver()));

            ServiceContainer.Register(() => new UserDigestService(
                new HttpUserDigestDriver(
                    host: userServicesURL
                //auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));

            //ServiceContainer.Register(() => new TicketsService(new MockTicketsDriver()));

            ServiceContainer.Register(() => new TicketsService(
                new HttpTicketsDriver(
                    host: userServicesURL
                //auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));

            ServiceContainer.Register(() => new SettingsService(new LAPhil.Settings.Realm.RealmDriver(
                path: settingsFilename
            )));

            //ServiceContainer.Register(() => new CacheService(new LAPhil.Cache.Memory.MemoryDriver(
            //    serializer: new JsonSerializerService()
            //)));

            ServiceContainer.Register(() => new CacheService(new LAPhil.Cache.Realm.RealmDriver(
                path: cacheFilename,
                serializer: new JsonSerializerService()
            )));

            ServiceContainer.Register(
                new Router(children: new List<Route> {

                    new Route(path: @"/concerts/:season/:slug/2018-01-05/", action: (request) => {
                        Log.Debug($"Invoked segment 'foo' {request.Params}");
                    })
            }));

            Log = ServiceContainer.Resolve<LoggingService>().GetLogger<AppDelegate>();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            SaveCookies();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            LoadCookies();
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.

            // in case the user changes their preferred time format.
            var timeService = ServiceContainer.Resolve<TimeService>();
            timeService.DetectTimeFormat();
        }

        public override void WillTerminate(UIApplication application)
        {
            SaveCookies();
        }

        public void SaveCookies() {

            var cookies = NSHttpCookieStorage.SharedStorage.Cookies;
            var cookieArray = new NSMutableArray();

            foreach(NSHttpCookie cookie in cookies) {

                cookieArray.Add((NSString)cookie.Name);
                var cookieProperties = new NSMutableDictionary();


                cookieProperties.SetValueForKey((NSString)cookie.Name, NSHttpCookie.KeyName);
                cookieProperties.SetValueForKey((NSString)cookie.Value, NSHttpCookie.KeyValue);
                cookieProperties.SetValueForKey((NSString)cookie.Domain, NSHttpCookie.KeyDomain);
                cookieProperties.SetValueForKey((NSString)cookie.Path, NSHttpCookie.KeyPath);
                cookieProperties.SetValueForKey(NSNumber.FromNUInt(cookie.Version), NSHttpCookie.KeyVersion);
                cookieProperties.SetValueForKey(new NSDate().AddSeconds(2629743), NSHttpCookie.KeyExpires);

                NSUserDefaults.StandardUserDefaults.SetValueForKey(cookieProperties, (NSString)cookie.Name);

            }
            NSUserDefaults.StandardUserDefaults.SetValueForKey(cookieArray, (NSString)"cookieArray");
            NSUserDefaults.StandardUserDefaults.Synchronize();

        }

        public void LoadCookies() {

            var arr = NSUserDefaults.StandardUserDefaults.ValueForKey((NSString)"cookieArray");
            if (arr == null) {
                return;
            }
            var cookieArray = (NSMutableArray)arr;
            if (cookieArray == null) {
                return;
            }
            for (nuint i = 0; i < cookieArray.Count; i++) {

                var cookieDictionary = (NSDictionary)NSUserDefaults.StandardUserDefaults.ValueForKey((NSString)cookieArray.GetItem<NSString>(i));
                var cookie = new NSHttpCookie(cookieDictionary);
                NSHttpCookieStorage.SharedStorage.SetCookie(cookie);
                              
            }

        }

        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Portrait;
        }

    }
}

