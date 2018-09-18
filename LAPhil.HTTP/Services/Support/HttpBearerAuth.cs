using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace LAPhil.HTTP
{
    public class HttpBearerAuth: IHttpAuth
    {
        public string Token { get; }
        public HttpBearerAuth(string token)
        {
            Token = token;
        }

        public Task Prepare(HttpRequestMessage request)
        {
            var credential = $"Bearer {Token}";
            request.Headers.Add(name: "Authorization", value: credential);

            return Task.CompletedTask;
        }
    }
}
