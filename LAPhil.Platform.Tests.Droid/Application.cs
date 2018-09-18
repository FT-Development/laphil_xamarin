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
using Xamarin.Forms;

namespace LAPhil.Droid
{

    [Application]
    public class Application : Android.App.Application, Android.App.Application.IActivityLifecycleCallbacks
    {
        ILog Log { get; set; }

        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            RegisterActivityLifecycleCallbacks(this);
            //UserModel.InitInstance(this);
            Forms.Init(this, null);

            ConfigureServices();
        }

        void ConfigureServices()
        {
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

            //ServiceContainer.Register(() => new EventService(
            //    new MockEventsDriver()
            //));

            ServiceContainer.Register(() => new EventService(
                new HttpEventsDriver(host: "http://laphil-dev.herokuapp.com")
            ));

            ServiceContainer.Register(() => new AuthService(new MockAuthDriver()));
            //ServiceContainer.Register(() => new AuthService(
            //    new HttpAuthDriver(
            //        host:"https://tickets-dev.laphil.com", 
            //        auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
            //    )
            //));

            ServiceContainer.Register(() => new UserDigestService(new MockUserDigestDriver()));
            //ServiceContainer.Register(() => new UserDigestService(
            //    new HttpUserDigestDriver(
            //        host: "https://tickets-dev.laphil.com",
            //        auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
            //    )
            //));

            ServiceContainer.Register(() => new TicketsService(new MockTicketsDriver()));
            //ServiceContainer.Register(() => new TicketsService(
            //    new HttpTicketsDriver(
            //        host: "https://tickets-dev.laphil.com",
            //        auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
            //    )
            //));

            ServiceContainer.Register(() => new SettingsService(new LAPhil.Settings.Realm.RealmDriver(
                path: settingsFilename
            )));

            ServiceContainer.Register(() => new CacheService(new LAPhil.Cache.Memory.MemoryDriver(
                serializer: new JsonSerializerService()
            )));

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