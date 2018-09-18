using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Xunit;
using LAPhil.Application;
using LAPhil.User;

namespace LAPhil.User.Tests.Platforms.iOS.Tests
{
    public class TicketServiceTests: IClassFixture<CredentialsFixture>
    {
        CredentialsFixture Credentials;

        public TicketServiceTests(CredentialsFixture credentials)
        {
            Credentials = credentials;
        }

        [Fact]
        public async Task GetTickets()
        {
            var loginService = ServiceContainer.Resolve<LoginService>();
            var ticketsService = ServiceContainer.Resolve<TicketsService>();


            var account = await loginService.Login(
                username: Credentials.Username,
                password: Credentials.Password
            );

            var tickets = await ticketsService.GetTickets(account);
            Assert.True(true);
        }

        [Fact]
        public async Task GetTicketDetail()
        {
            var loginService = ServiceContainer.Resolve<LoginService>();
            var ticketsService = ServiceContainer.Resolve<TicketsService>();


            var account = await loginService.Login(
                username: Credentials.Username,
                password: Credentials.Password
            );

            var tickets = await ticketsService.GetTickets(account);
            var detail = await ticketsService.GetTicketDetail(account, tickets[0]);

            Assert.True(true);
        }
    }
}
