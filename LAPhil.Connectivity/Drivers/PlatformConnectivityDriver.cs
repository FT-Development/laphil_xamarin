using System;
using System.Reactive.Subjects;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using LAPhil.Logging;
using LAPhil.Application;


namespace LAPhil.Connectivity
{
    public class PlatformConnectivityDriver: IConnectivityDriver
    {
        static IConnectivity host = CrossConnectivity.Current;
        static Logger<ConnectivityService> Log = ServiceContainer.Resolve<LoggingService>().GetLogger<ConnectivityService>();

        public readonly BehaviorSubject<bool> IsConnectedSubject;
        public bool IsConnected => IsConnectedSubject.Value;


        public PlatformConnectivityDriver()
        {
            IsConnectedSubject = new BehaviorSubject<bool>(host.IsConnected);
            host.ConnectivityChanged += onConnectivityChanged;
            Log.Info("Listening for connectivity changes");
        }

        void onConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Log.Info("Connectivity Enabled '{IsConnected}'", e.IsConnected);
            IsConnectedSubject.OnNext(e.IsConnected);
        }
    }
}
