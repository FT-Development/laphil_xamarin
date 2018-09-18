using System.Reactive.Subjects;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using LAPhil.Application;
using LAPhil.Logging;


namespace LAPhil.Connectivity
{
    public class ConnectivityService
    {
        IConnectivityDriver Driver;
        public bool IsConnected => Driver.IsConnected;

        public ConnectivityService(IConnectivityDriver driver)
        {
            Driver = driver;
        }
    }
}
