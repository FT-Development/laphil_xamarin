using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.Connectivity;
using LAPhil.HTTP;
using LAPhil.Cache;
using LAPhil.Events;
using LAPhil.User;
using LAPhil.Auth;
using LAPhil.Cache.Realm;
using LAPhil.Analytics;
using LAPhil.Routing;
using LAPhil.Platform;
using LAPhil.Settings;
using Android.OS;
using LAPhil.Urls;
using FabricSdk;
using CrashlyticsKit;
using Xamarin.Forms;
using UrbanAirship.RichPush;
using UrbanAirship.Push;
using UrbanAirship;

namespace HollywoodBowl.Droid
{
    [Application]
    public class Application : Android.App.Application, Android.App.Application.IActivityLifecycleCallbacks
    {
        ILog Log { get; set; }
        GimbalPlaceEventListener gimbalPlaceEventListener;

        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            Forms.Init(this, null);
            Crashlytics.Instance.Initialize();
            Fabric.Instance.Debug = true;
            Fabric.Instance.Initialize(this);
           
            ConfigureServices();

            AirshipConfigOptions options = new AirshipConfigOptions.Builder()
              .SetInProduction(true)
              .SetDevelopmentAppKey("IUNz7rBvRXiNRPstvGxpGg")
              .SetDevelopmentAppSecret("wawJWxFwTOW3n5J5PMUgcg")
              .SetProductionAppKey("IUNz7rBvRXiNRPstvGxpGg")
              .SetProductionAppSecret("wawJWxFwTOW3n5J5PMUgcg")
              .SetFcmSenderId("55941271460")
              .Build();
            UrbanAirship.UAirship.TakeOff(this, options, (UAirship airship) =>
            {
                airship.PushManager.PushEnabled = true;
                airship.PushManager.UserNotificationsEnabled = true;
                airship.LocationManager.LocationUpdatesEnabled = true;

            });
            UrbanAirship.UAirship.Shared().LocationManager.LocationUpdatesEnabled = true;

            Com.Gimbal.Android.Gimbal.SetApiKey(this, "62939f23-c4e7-43eb-8eae-2bed104aa858");
            //this class will log beacon hits and place entry/exits
            gimbalPlaceEventListener = new GimbalPlaceEventListener();
            //add it as a listener to the placemanager
            Com.Gimbal.Android.PlaceManager.Instance.AddListener(gimbalPlaceEventListener);
            //for local notifications, this will (i believe) set up messaging, though more work would have to be done for real GCM setup
            Com.Gimbal.Android.CommunicationManager.Instance.StartReceivingCommunications();
            //and tell gimbal to start looking
            Com.Gimbal.Android.PlaceManager.Instance.StartMonitoring();

        }

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


            var cacheStoragePath = PathService.CachePath;
            var settingsStoragePath = PathService.SettingsPath;

            var cacheFilename = Path.Combine(cacheStoragePath, "cache.realm");
            var settingsFilename = Path.Combine(settingsStoragePath, "settings.realm");

            PathService.CreatePath(cacheStoragePath);
            PathService.CreatePath(settingsStoragePath);


            // Logging service must be initialized first and MUST NOT be in a lambda
            // create the concrete instance.

            ServiceContainer.Register(new LoggingService(new PlatfromLogger()));

            // !!! ANDROID ONLY SERVICE !!!
            ServiceContainer.Register(() => new CurrentActivityService());
            // !!! DON'T JUST COPY AND PASTE THIS INTO iOS !!!!

            ServiceContainer.Register(() => new ConnectivityService(new PlatformConnectivityDriver()));
            ServiceContainer.Register(() => new HttpService(timeout: 6.05));

            ServiceContainer.Register(() => new AnalyticsService(new MockAnalyticsDriver()));

            ServiceContainer.Register(() => new LAPhilUrlService(
                webDining: "http://www.hollywoodbowl.com/plan-your-visit/when-youre-here/dining/?mobile_webview=true",
                webChart: "http://www.hollywoodbowl.com/plan-your-visit/when-youre-here/seating-chart/?mobile_webview=true",
                webAccessibility: "http://www.hollywoodbowl.com/plan-your-visit/when-youre-here/accessibility-info/?mobile_webview=true",
                webMetroBus: "http://www.hollywoodbowl.com/plan-your-visit/getting-here/?mobile_webview=true",
                webRegister: "http://www.hollywoodbowl.com/account/register?mobile_webview=true",
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
            ServiceContainer.Register(() => new LoginService(ApplicationContext));
            ServiceContainer.Register(() =>
                (ILoginService)ServiceContainer.Resolve<LoginService>()
            );

            //ServiceContainer.Register(() => new EventService(
            //    new MockEventsDriver()
            //));

            ServiceContainer.Register(() => new EventService(
                new HttpEventsDriver(host: servicesURL)
            ));

            //ServiceContainer.Register(() => new AuthService(new MockAuthDriver()));
            ServiceContainer.Register(() => new AuthService(
              new HttpAuthDriver(
                  host: userServicesURL
              )
          ));


         //   ServiceContainer.Register(() => new UserDigestService(new MockUserDigestDriver()));
            ServiceContainer.Register(() => new UserDigestService(
                new HttpUserDigestDriver(
                    host: userServicesURL
                )
            ));

            ServiceContainer.Register(() => new TicketsService(
               new HttpTicketsDriver(
                   host: userServicesURL
               //auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
               )
           ));

            ServiceContainer.Register(() => new SettingsService(new LAPhil.Settings.Realm.RealmDriver(
                path: settingsFilename
            )));

            ServiceContainer.Register(() => new CacheService(new LAPhil.Cache.Realm.RealmDriver(
                path: cacheFilename,
                serializer: new JsonSerializerService()
            )));

            //ServiceContainer.Register(() => new CacheService(new LAPhil.Cache.Memory.MemoryDriver(
            //  serializer: new JsonSerializerService()
            //)));

            ServiceContainer.Register(
                new Router(children: new List<Route> {

                    new Route(path: @"/concerts/:season/:slug/2018-01-05/", action: (request) => {
                        Log.Debug($"Invoked segment 'foo' {request.Params}");
                    })
            }));

            Log = ServiceContainer.Resolve<LoggingService>().GetLogger<Application>();
        }

        public void OnActivityDestroyed(Activity activity) { }
        public void OnActivityPaused(Activity activity) { }
        public void OnActivitySaveInstanceState(Activity activity, Bundle outState) { }
        public void OnActivityStopped(Activity activity) { }


        public void OnActivityResumed(Activity activity)
        {
            ServiceContainer.Resolve<CurrentActivityService>().Activity = activity;
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            ServiceContainer.Resolve<CurrentActivityService>().Activity = activity;
        }

        public void OnActivityStarted(Activity activity)
        {
            ServiceContainer.Resolve<CurrentActivityService>().Activity = activity;
        }


    }
}
