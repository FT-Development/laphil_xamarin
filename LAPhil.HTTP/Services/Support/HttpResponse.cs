using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


namespace LAPhil.HTTP
{
    public class HttpResponse
    {
        public HttpResponseMessage Response { get; private set; }
        public HttpResponse(HttpResponseMessage response)
        {
            Response = response;
        }

        public async Task<string> StringAsync() => await Response.Content.ReadAsStringAsync();
        public async Task<byte[]> ByteArrayAsync() => await Response.Content.ReadAsByteArrayAsync();
        public async Task<T> JsonAsync<T>()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(await StringAsync());
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
