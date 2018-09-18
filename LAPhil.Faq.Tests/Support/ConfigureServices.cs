using System;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.HTTP;
using LAPhil.Cache;
using LAPhil.Cache.Memory;
using LAPhil.Platform;
using LAPhil.Connectivity;
using LAPhil.Faq;


namespace LAPhil.Faq.Tests
{
    public class ConfigureServices : IDisposable
    {

        public ConfigureServices()
        {
            Configure();
        }

        void Configure()
        {
            Console.WriteLine("CONFIGURING SERVICES");
            ServiceContainer.Register(new LoggingService(new PlatfromLogger()));

            ServiceContainer.Register(() => new HttpService(timeout: 6.05));
            ServiceContainer.Register(() => new ConnectivityService(new MockConnectivityDriver()));

            ServiceContainer.Register(() => new CacheService(new MemoryDriver(
                serializer: new JsonSerializerService()
            )));

            ServiceContainer.Register(() => new FaqService(
                new MockFaqDriver()
            ));

            //ServiceContainer.Register(() => new FaqService(
            //    new HttpFaqDriver(host: "http://laphil-dev.herokuapp.com")
            //));
        }


        public void Dispose()
        {
            ServiceContainer.Clear();
        }
    }
}
