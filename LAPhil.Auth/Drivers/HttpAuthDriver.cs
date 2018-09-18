using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using LAPhil.HTTP;
using LAPhil.Logging;
using LAPhil.Application;


namespace LAPhil.Auth
{

    public class HttpAuthDriver: IAuthDriver
    {
        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<HttpAuthDriver>();
        HttpService Http = ServiceContainer.Resolve<HttpService>();
        readonly string Host;
        readonly IHttpAuth Authorization;

        public HttpAuthDriver(string host)
        {
            Host = host;
        }

        public HttpAuthDriver(string host, IHttpAuth auth)
        {
            Host = host;
            Authorization = auth;
        }


        public IObservable<AuthResponse> RefreshToken(string token, string refreshToken)
        {
            /*
             *  curl -X POST --data "token=TOKEN&refresh_token=REFRESH_TOKEN" https://tickets-dev.laphil.com/api/jwt/refresh-authorization
             */

            return Observable.FromAsync(async () =>
            {
                JWT newToken;

                var url = $"{Host}/api/jwt/refresh-authorization";
                var data = new Dictionary<string, string>
                {
                    ["token"] = token,
                    ["refresh_token"] = refreshToken
                };


                var headers = new Dictionary<string, string>();

                var result = await Http.PostResultAsync<HttpAuthResponse>(
                    url: url, data: data, headers: headers, auth: Authorization
                );

                if (result.IsFailure)
                {
                    if (result.Error is Unauthorized)
                        throw new InvalidToken();
                    throw result.Error;
                }

                try
                {
                    newToken = JWT.FromTokenString(result.Value.Token);
                }
                catch (ArgumentException e)
                {
                    throw e;
                }

                var response = new AuthResponse
                {
                    Token = newToken,
                    RefreshToken = result.Value.RefreshToken,
                };

                return response;
            });
        }

        public IObservable<AuthResponse> Login(string username, string password)
        {
            /* 
             * 
             * curl -X POST --data "username=XXX&password=YYY" https://tickets-dev.laphil.com/api/jwt/authorization
             * curl -X GET https://tickets-dev.laphil.com/api/jwt/anonymous 
             * 
             * code 200 - user has been authorised, token in response
             * code 400 - user has not been authorised, missing username or password
             * code 401 - user has not been authorised, invalid username or password
             * 
             * 401
             * {
             *    "error": "Incorrect username or password"
             * }
             * */

            return Observable.FromAsync(async () =>
            {
                JWT token;

                var url = $"{Host}/api/jwt/authorization";
                var data = new Dictionary<string, string>
                {
                    ["username"] = username,
                    ["password"] = password
                };

                var headers = new Dictionary<string, string>();

                var result = await Http.PostResultAsync<HttpAuthResponse>(
                    url: url, data: data, headers: headers, auth: Authorization
                );

                if(result.IsFailure)
                {
                    if(result.Error is Unauthorized)
                        throw new InvalidUsernameOrPassword();

                    throw result.Error;
                }

                try
                {
                    token = JWT.FromTokenString(result.Value.Token);
                } 
                catch(ArgumentException e)
                {
                    throw e; 
                }

                var response  = new AuthResponse
                {
                    Token = token,
                    RefreshToken = result.Value.RefreshToken,

                };

                return response;

            });
        }
    }
}
