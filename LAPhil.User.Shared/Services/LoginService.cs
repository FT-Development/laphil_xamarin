using System;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Reactive.Subjects;
using LAPhil.Auth;
using LAPhil.Application;


namespace LAPhil.User
{
    
    public partial class LoginService: ILoginService
    {
        public sealed class _Rx
        {
            public AsyncSubject<Account> Login = new AsyncSubject<Account>();

            internal _Rx()
            {
                Reset();
            }

            internal void Reset()
            {
                Login = new AsyncSubject<Account>();
            }

            public void Complete(Account account)
            {
                Login.OnNext(account);
                Login.OnCompleted();
                Reset();
            }
        }

        const string ServiceId = "blocksoffice";

        _Rx _rx = new _Rx();
        public _Rx Rx { get => _rx; }

        Account _CurrentAccount = null;


        Xamarin.Auth.Account ConvertToAuthAccount(Account account)
        {
            // Xamarin.Auth.Acccount will not persist with 
            // null values. At the time of development, "refreshToken"
            // could be null. So we need to check for that.

            var props = new Dictionary<string, string>
            {
                ["token"] = account.Token.TokenString,
            };

            if (account.Token.RefreshToken != null)
                props["refresh_token"] = account.Token.RefreshToken;

            var result = new Xamarin.Auth.Account(username: account.Username, properties: props);
            return result;
        }

        Account ConvertToAccount(Xamarin.Auth.Account account)
        {
            var token = JWT.FromTokenString(account.Properties["token"]);

            if (account.Properties.TryGetValue("refresh_token", out string refreshToken) && refreshToken != null)
                token.RefreshToken = refreshToken;

            var result = new Account
            {
                Token = token,
                Username = account.Username,
            };

            return result;
        }

        public void LoginCancel()
        {
            Rx.Complete(null);
        }

        public IObservable<Account> RefreshToken(Account account)
        {

            return Observable.FromAsync(async () => { 
                AuthResponse response = null;

                var token = account.Token.TokenString;
                var refreshToken = account.Token.RefreshToken;

                var authService = ServiceContainer.Resolve<AuthService>();

                try
                {
                    response = await authService.RefreshToken(token: token, refreshToken: refreshToken);
                }
                catch (Exception e)
                {
                    if (e is LAPhil.Auth.InvalidToken)
                        throw new LAPhil.User.InvalidToken();

                    throw new UnknownAuthException();
                }

                var result = new Account
                {
                    Username = account.Username,
                    Token = response.Token,
                };

                result.Token.RefreshToken = response.RefreshToken;

                _CurrentAccount = result;
                return result;
            });
        }

        public void SetCurrentAccount(Account account)
        {
            _CurrentAccount = account;
        }

        public IObservable<Account> Login(string username, string password, string facebookEmail)
        {
            return Observable.FromAsync(async () =>
            {
                AuthResponse response = null;
                var authService = ServiceContainer.Resolve<AuthService>();

                try
                {
                    response = await authService.Login(username, password, facebookEmail);
                }
                catch(Exception e)
                {
                    if (e is LAPhil.Auth.InvalidUsernameOrPassword)
                        throw new LAPhil.User.InvalidUsernameOrPassword();

                    throw new UnknownAuthException();
                }

                var result = new Account
                {
                    Username = username,
                    Token = response.Token,
                };

                result.Token.RefreshToken = response.RefreshToken;
                return result;
            });
        }

    }
}
