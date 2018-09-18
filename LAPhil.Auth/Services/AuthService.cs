using System;


namespace LAPhil.Auth
{
    public class AuthService
    {
        public IAuthDriver Driver { get; internal set; }

        public AuthService(IAuthDriver driver)
        {
            Driver = driver;
        }

        public IObservable<AuthResponse> RefreshToken(string token, string refreshToken)
        {
            return Driver.RefreshToken(token: token, refreshToken: refreshToken);
        }

        public IObservable<AuthResponse> Login(string username, string password)
        {
            return Driver.Login(username, password);
        }

    }
}
