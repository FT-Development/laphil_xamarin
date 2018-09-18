using System;


namespace LAPhil.Auth
{
    public interface IAuthDriver
    {
        IObservable<AuthResponse> Login(string username, string password);
        IObservable<AuthResponse> RefreshToken(string token, string refreshToken);
    }
}
