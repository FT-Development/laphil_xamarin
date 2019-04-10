using System;
using System.IO;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using Android.OS;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.Connectivity;
using LAPhil.HTTP;
using LAPhil.Cache;
using LAPhil.Analytics;
using LAPhil.Analytics.Platform;
using LAPhil.Routing;
using LAPhil.Settings;
using LAPhil.Events;
using LAPhil.User;
using LAPhil.Platform;
using LAPhil.Auth;
using LAPhil.Urls;
using Xamarin.Forms;
using UrbanAirship;
using FabricSdk;
using CrashlyticsKit;

namespace LAPhil.Droid
{
    [Application]
    public class Application : Android.App.Application, Android.App.Application.IActivityLifecycleCallbacks
    {
        ILog Log { get; set; }

        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        private GimbalPlaceEventListener gimbalPlaceEventListener;
        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
          
            Forms.Init(this,null);
            Crashlytics.Instance.Initialize();
            Fabric.Instance.Debug = true;
            Fabric.Instance.Initialize(this);
          
            ConfigureServices();

            //AirshipConfigOptions options = new AirshipConfigOptions.Builder()
                                                                   //.SetInProduction(true)
                                                                   //.SetDevelopmentAppKey("47w7BrouRnOSP1wVKcwuBQ")
                                                                   //.SetDevelopmentAppSecret("2s1HvxrTTBWuu11wnHM_xA")
                                                                   //.SetProductionAppKey("47w7BrouRnOSP1wVKcwuBQ")
                                                                   //.SetProductionAppSecret("2s1HvxrTTBWuu11wnHM_xA")
                                                                   //.SetFcmSenderId("55941271460")
                                                                   //.Build();

            //AirshipConfigOptions options = new AirshipConfigOptions.Builder()
                                                                   //.SetInProduction(true)
                                                                   //.SetDevelopmentAppKey("GWu1P24IR4W-4ojRsVvZ3g")
                                                                   //.SetDevelopmentAppSecret("kh889bIzT4eGZfmJAFjvaA")
                                                                   //.SetProductionAppKey("GWu1P24IR4W-4ojRsVvZ3g")
                                                                   //.SetProductionAppSecret("kh889bIzT4eGZfmJAFjvaA")
                                                                   //.SetFcmSenderId("265969896159")
                                                                   //.Build();

            //UrbanAirship.UAirship.TakeOff(this, options, (UAirship airship) =>
            //{
            //    airship.PushManager.PushEnabled = true;
            //    airship.PushManager.UserNotificationsEnabled = true;
            //    airship.LocationManager.LocationUpdatesEnabled = true;

            //});


            Com.Gimbal.Android.Gimbal.SetApiKey(this, "06368b6b-e8a4-4094-8edc-9f6e7ff543e5");//TODO: 
            //Com.Gimbal.Android.Gimbal.SetApiKey(this, "be6e6478-188a-4598-94ce-8a941de79bba");

            gimbalPlaceEventListener = new GimbalPlaceEventListener();
            Com.Gimbal.Android.PlaceManager.Instance.AddListener(gimbalPlaceEventListener);

        }
       
        void ConfigureServices()
        {
                // or in any reference assemblies           

                var servicesURL = "https://www.laphil.com";
                 // var serviceURL = "http://laphil-staging.herokuapp.com";

                var userServicesURL = "https://my.laphil.com";
                //var userServicesURL = "https://tickets-dev.laphil.com";

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

                ServiceContainer.Register(() => new AnalyticsService(new KochavaDriver(appId: "kola-phil-app-elo8sqaoz")));
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
            ServiceContainer.Register(() => new LoginService(ApplicationContext));
            ServiceContainer.Register(() =>
                (ILoginService)ServiceContainer.Resolve<LoginService>()
            );
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
                aboutUs: "https://www.laphil.com/about/la-phil/",
                privacyPolicy: "https://www.laphil.com/privacy-policy/",
                appTitle: "LA Phil"
            ));

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

           // ServiceContainer.Register(() => new UserDigestService(new MockUserDigestDriver()));
            ServiceContainer.Register(() => new UserDigestService(
                new HttpUserDigestDriver(
                    host: userServicesURL
                )
            ));

           // ServiceContainer.Register(() => new TicketsService(new MockTicketsDriver()));
            ServiceContainer.Register(() => new TicketsService(
                new HttpTicketsDriver(
                    host: userServicesURL
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
            //    serializer: new JsonSerializerService()
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
