using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;
using LAPhil.Application;
using LAPhil.Events;


namespace LAPhil.Platform.Tests.Droid.Tests
{
    public class Events
    {
        public Events()
        {
        }

        [Fact]
        public Task FetchEvents()
        {
            var tcs = new TaskCompletionSource<object>();
            var eventsService = ServiceContainer.Resolve<EventService>();

            eventsService
                .CurrentEvents()
                .Subscribe(results =>
                {
                    Assert.True(results.Length > 0);
                    tcs.SetResult(null);
                });

            return tcs.Task;
        }

    }
}
