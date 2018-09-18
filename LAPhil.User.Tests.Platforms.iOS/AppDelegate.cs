using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation;
using UIKit;
using Xunit.Runner;
using Xunit.Sdk;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.Connectivity;
using LAPhil.HTTP;
using LAPhil.Cache;
using LAPhil.Cache.Memory;
//using LAPhil.Analytics;
//using LAPhil.Routing;
//using LAPhil.Settings;
using LAPhil.Auth;
//using LAPhil.Events;
using LAPhil.User;
using LAPhil.Platform;



namespace LAPhil.User.Tests.Platforms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : RunnerAppDelegate
    {

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            
            // We need this to ensure the execution assembly is part of the app bundle
            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);


            // tests can be inside the main assembly
            AddTestAssembly(Assembly.GetExecutingAssembly());
            // otherwise you need to ensure that the test assemblies will 
            // become part of the app bundle
            //AddTestAssembly(typeof(PortableTests).Assembly);

#if false
            // you can use the default or set your own custom writer (e.g. save to web site and tweet it ;-)
            Writer = new TcpTextWriter ("10.0.1.2", 16384);
            // start running the test suites as soon as the application is loaded
            AutoStart = true;
            // crash the application (to ensure it's ended) and return to springboard
            TerminateAfterExecution = true;
#endif
            ConfigureServices();
            return base.FinishedLaunching(app, options);


        }

        void ConfigureServices()
        {

            var serviceURL = "http://laphil-staging.herokuapp.com";
            //var serviceURL = "https://tickets-dev.laphil.com";


            var cacheStoragePath = PathService.CachePath;
            var settingsStoragePath = PathService.SettingsPath;

            var cacheFilename = Path.Combine(cacheStoragePath, "cache.realm");
            var settingsFilename = Path.Combine(settingsStoragePath, "settings.realm");

            PathService.CreatePath(cacheStoragePath);
            PathService.CreatePath(settingsStoragePath);

            ServiceContainer.Register(new LoggingService(new PlatfromLogger()));
            ServiceContainer.Register(() => new ConnectivityService(new MockConnectivityDriver()));
            ServiceContainer.Register(() => new HttpService(timeout: 30.05));

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
                (ILoginService) ServiceContainer.Resolve<LoginService>()
            );


            ServiceContainer.Register(() => new CacheService(new MemoryDriver(
                serializer: new JsonSerializerService()
            )));

            //ServiceContainer.Register(() => new AuthService(new MockAuthDriver()));
            ServiceContainer.Register(() => new AuthService(
                new HttpAuthDriver(
                    host:serviceURL, 
                    auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));

            //ServiceContainer.Register(() => new UserDigestService(new MockUserDigestDriver()));
            ServiceContainer.Register(() => new UserDigestService(
                new HttpUserDigestDriver(
                    host: serviceURL,
                    auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));

            //ServiceContainer.Register(() => new TicketsService(new MockTicketsDriver()));
            ServiceContainer.Register(() => new TicketsService(
                new HttpTicketsDriver(
                    host: serviceURL,
                    auth: new HttpBasicAuth(username: "lapatester", password: "p@ssw0rd")
                )
            ));
        }


    }
}