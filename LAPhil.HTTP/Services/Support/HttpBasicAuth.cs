using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace LAPhil.HTTP
{
    public class HttpBasicAuth : IHttpAuth
    {
        public string Username { get; }
        public string Password { get; }

        public HttpBasicAuth(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public Task Prepare(HttpRequestMessage request)
        {
            var encoded = Encoding.GetEncoding("ISO-8859-1").GetBytes($"{Username}:{Password}");
            var credential = $"Basic {Convert.ToBase64String(encoded)}";

            request.Headers.Add(name: "Authorization", value: credential);

            return Task.CompletedTask;
        }
    }
}
