using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LAPhil.HTTP
{
    public interface IHttpAuth
    {
        Task Prepare(HttpRequestMessage request);
    }
}
