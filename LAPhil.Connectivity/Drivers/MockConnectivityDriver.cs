using System;
using LAPhil.Logging;
using LAPhil.Application;


namespace LAPhil.Connectivity
{
    public class MockConnectivityDriver: IConnectivityDriver
    {
        static ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<MockConnectivityDriver>();

        Func<bool> _connectivityTest;
        public bool IsConnected => _connectivityTest();

        public MockConnectivityDriver(): this(connectivityTest: () => true)
        {
            
        }

        public MockConnectivityDriver(Func<bool> connectivityTest)
        {
            _connectivityTest = connectivityTest;
            Log.Info("Mock Connectivity Driver Initialized");
        }
    }
}
