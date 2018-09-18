using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Xunit;
using LAPhil.Application;
using LAPhil.Events;


namespace LAPhil.Events.Tests
{
    public class Integration: IClassFixture<ConfigureServices>
    {
        public Integration(ConfigureServices services){}

        [Fact]
        public async Task FetchEvents()
        {
            var eventsService = ServiceContainer.Resolve<EventService>();
            var result = await eventsService.CurrentEvents();

            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task FetchEventDetail()
        {
            var eventsService = ServiceContainer.Resolve<EventService>();
            var list = await eventsService.CurrentEvents();

            Assert.True(list.Length > 0);

            var detail = await eventsService.EventDetail(list[0]);

            Assert.True(detail != null);
            Assert.True(list[0].Program.Name != detail.Program.Name);
        }
    }
}
