using System;
using System.Threading.Tasks;
using System.Reactive.Linq;


namespace LAPhil.Auth
{
    public class MockAuthDriver: IAuthDriver
    {
        public MockAuthDriver()
        {
        
        }

        public IObservable<AuthResponse> Login(string username, string password, string facebookEmail)
        {
            return Observable.FromAsync(async () =>
            {
                await Task.Delay(60);
                return new AuthResponse
                {
                    Token = JWT.FromTokenString("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1MTM2MjQyMTEsInNlc3Npb24iOiJhNjAzZGJjM2U0MWUxMWU3ODBiZDE0MDJlYzMxNmZjMzAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwIn0.eRhs12RXesuyAelAisyMLpjia68G8tC3sWVPFyiCWKM"),
                    RefreshToken = "1234"
                };
            });
        }

        public IObservable<AuthResponse> RefreshToken(string token, string refreshToken)
        {
            return Observable.FromAsync(async () =>
            {
                await Task.Delay(60);
                return new AuthResponse
                {
                    Token = JWT.FromTokenString("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1MTM2MjQyMTEsInNlc3Npb24iOiJhNjAzZGJjM2U0MWUxMWU3ODBiZDE0MDJlYzMxNmZjMzAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwIn0.eRhs12RXesuyAelAisyMLpjia68G8tC3sWVPFyiCWKM"),
                    RefreshToken = "1234"
                };
            });
        }
    }
}
