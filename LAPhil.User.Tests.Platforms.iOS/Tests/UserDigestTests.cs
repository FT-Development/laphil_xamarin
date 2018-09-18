using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Xunit;
using LAPhil.Application;
using LAPhil.User;


namespace LAPhil.User.Tests.Platforms.iOS.Tests
{
    public class UserDigestTests: IClassFixture<CredentialsFixture>
    {
        CredentialsFixture Credentials;

        public UserDigestTests(CredentialsFixture credentials)
        {
            Credentials = credentials;
        }

        [Fact]
        public async Task GetUserDigest()
        {
            var loginService = ServiceContainer.Resolve<LoginService>();
            var userDigestService = ServiceContainer.Resolve<UserDigestService>();

            var account = await loginService.Login(
                username: Credentials.Username,
                password: Credentials.Password
            );

            var digest = await userDigestService.GetUserDigest(account);

            Assert.True(digest != null);
            Assert.True(digest.EmailAddresses[0].Address == Credentials.Username);
        }
    }
}
