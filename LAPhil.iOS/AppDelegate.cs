using CrashlyticsKit;
using FabricSdk;
using Foundation;
using UIKit;
using System.IO;
using System.Collections.Generic;
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

using System;
using LAPhilShared.Views.UrbanAirship;
using UrbanAirship;


namespace LAPhil.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
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

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            setupUrbanAirship();

            Forms.Init();
            CrashlyticsKit.Crashlytics.Instance.Initialize();
            FabricSdk.Fabric.Instance.Initialize();
            ConfigureServices();
            Firebase.Core.App.Configure();
            //var container = Google.TagManager.Manager.GetInstance.GetContainer("GTM-KLLCRTG");
            return true;
        }



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

            UAirship.Push().WeakRegistrationDelegate = (UrbanAirship.IUARegistrationDelegate)this;

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
            // var servicesURL = "https://laphil-staging.herokuapp.com";
            //var servicesURL = "https://www-dev.laphil.com";
            // var servicesURL = "https://www-dev.hollywoodbowl.com";
            var servicesURL = "https://www.laphil.com";

            //var userServicesURL = "https://tickets-dev.laphil.com";
            var userServicesURL = "https://my.laphil.com";



            // Note that the laphil dev services require basic auth
            // see below where it's commented out and uncomment it
            // if you switch to the dev services.
            // var userServicesURL = "https://tickets-dev.laphil.com";

            var cacheStoragePath = PathService.CachePath;
            var settingsStoragePath = PathService.SettingsPath;

            var cacheFilename = Path.Combine(cacheStoragePath, "cache.realm");
            var settingsFilename = Path.Combine(settingsStoragePath, "settings.realm");

            PathService.CreatePath(cacheStoragePath);
            PathService.CreatePath(settingsStoragePath);

            ServiceContainer.Register(new LoggingService(new PlatfromLogger()));
            ServiceContainer.Register(() => new ConnectivityService(new PlatformConnectivityDriver()));
            ServiceContainer.Register(() => new HttpService(timeout: 6.05));

            ServiceContainer.Register(() => new AnalyticsService(new KochavaDriver(appId: "kola-phil-app-elo8sqaoz")));

            ServiceContainer.Register(() => new TimeService());
            ServiceContainer.Register(new LoadingService());

            ServiceContainer.Register(() => new LAPhilUrlService(
                webDining: "https://www.laphil.com/plan-your-visit/when-youre-here/dining/?mobile_webview=true",
                webChart: "https://www.laphil.com/plan-your-visit/when-youre-here/seating-chart/?mobile_webview=true",
                webAccessibility: "https://www.laphil.com/plan-your-visit/when-youre-here/accessibility-info/?mobile_webview=true",
                webMetroBus: "https://www.laphil.com/plan-your-visit/getting-here/?mobile_webview=true",
                webRegister: "https://my.laphil.com/account/register?mobile_webview=true",
                webTickets: "https://my.laphil.com/secure/account?mobile_webview=true",
                webSupportUs: "https://my.laphil.com/support/membership?mobile_webview=true",
                webGettingHere: "https://www.laphil.com/plan-your-visit/getting-here/?mobile_webview=true",
                appStore: "https://itunes.apple.com/us/app/la-phil/id394783027?mt=8",
                webFoodWine: "https://www-prod.hollywoodbowl.com/plan-your-visit/food-wine/?mobile_webview=true",
                faq: "https://www.laphil.com/plan-your-visit/faq/?mobile_webview=true",
                centennial: "http://campaign.laphil.com/?mobile_webview=true",
                makeAGift: "https://my.laphil.com/support/membership?mobile_webview=true",
                corporateSponsorship: "https://www.laphil.com/support-us/corporate-sponsorship/?mobile_webview=true",
                otherWaysToSupport: "https://www.laphil.com/support-us/other-ways-support/?mobile_webview=true",
                myAccountDetails: "https://my.laphil.com/secure/account/update?mobile_webview=true",
                webSeatingChart: "https://www.laphil.com/plan-your-visit/when-youre-here/seating-chart/?mobile_webview=true",
                webForgotPassword: "https://my.laphil.com/account/forgottenpassword?mobile_webview=true",
                laStore: "https://www.laphilstore.com",
                bowlStore: "https://www.laphilstore.com/exclusives/hollywood-bowl.html",
                mobileOrdering: "https://hollywoodbowl.splickit.com",
                appTitle: "LA Phil"
            ));


            //ServiceContainer.Register(() => new AnalyticsService(new MockAnalyticsDriver()));

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

            //Harish_feb12
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

            //Harish_feb12
            //ServiceContainer.Register(() => new AuthService(new MockAuthDriver()));

            ServiceContainer.Register(() => new AuthService(
                new HttpAuthDriver(
                    host: userServicesURL
                    //,auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));

            //Harish_feb12
            //ServiceContainer.Register(() => new UserDigestService(new MockUserDigestDriver()));

            ServiceContainer.Register(() => new UserDigestService(
                new HttpUserDigestDriver(
                    host: userServicesURL
                    //,auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));

            //Harish_feb12
            //ServiceContainer.Register(() => new TicketsService(new MockTicketsDriver()));

            ServiceContainer.Register(() => new TicketsService(
                new HttpTicketsDriver(
                    host: userServicesURL
                    //,auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
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
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
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
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

