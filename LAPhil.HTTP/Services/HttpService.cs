using System;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

using System.Net.Http;
using Newtonsoft.Json;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.Connectivity;



namespace LAPhil.HTTP
{
    
    public class HttpService
    {
        // https://docs.microsoft.com/en-us/azure/architecture/antipatterns/improper-instantiation/
        private readonly HttpClient HttpClient;
        Logger<HttpService> Log = ServiceContainer.Resolve<LoggingService>().GetLogger<HttpService>();
        ConnectivityService ConnectivityService = ServiceContainer.Resolve<ConnectivityService>();

        public double Timeout { get; private set; }

        public HttpService(double timeout = 6.05)
        {
            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromSeconds(timeout);
        }

        async Task<Result<T>> MakeJsonResult<T>(HttpResponse response)
        {
            try
            {
                return new Result<T>(await response.JsonAsync<T>());
            }
            catch (Exception e)
            {
                Log.Warn(e.Message);
                return new Result<T>(e);
            }
        }

        public async Task<Result<T>> GetResultAsync<T>(string url, Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            HttpResponse response = null;

            try
            {
                response = await GetAsync(url: url, parameters: parameters, headers: headers, auth: auth);
            }
            catch (Exception ex)
            {
                return new Result<T>(ex);
            }

            return await MakeJsonResult<T>(response);
        }

        public async Task<Result<T>> PostResultAsync<T>(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, object> files = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            HttpResponse response = null;

            try
            {
                response = await PostAsync(url: url, data: data, json: json, files: files, headers: headers, auth: auth);
            }
            catch (Exception ex)
            {
                return new Result<T>(ex);
            }

            return await MakeJsonResult<T>(response);
        }

        public async Task<Result<T>> PutResultAsync<T>(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            HttpResponse response = null;

            try
            {
                response = await PutAsync(url: url, data: data, json: json, headers: headers, auth: auth);
            }
            catch (Exception ex)
            {
                return new Result<T>(ex);
            }

            return await MakeJsonResult<T>(response);
        }

        public async Task<HttpResponse> GetAsync(string url, Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            HttpResponseMessage response = await SendGetAsync(url: url, parameters: parameters, headers: headers, auth: auth);
            return new HttpResponse(response);
        }

        public async Task<HttpResponse> PostAsync(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, object> files = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            HttpResponseMessage response = await SendPostAsync(url: url, data: data, json: json, files: files, headers: headers, auth: auth);
            return new HttpResponse(response);
        }

        public async Task<HttpResponse> PutAsync(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            HttpResponseMessage response = await SendPutAsync(url: url, data: data, json: json, headers: headers, auth: auth);
            return new HttpResponse(response);
        }

        async Task<HttpResponseMessage> SendPutAsync(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            appendHeaders(request: request, headers: headers);

            if (data == null && json != null)
                prepareJson(request: request, json: json);
            else if (data != null)
                prepareUrlEncoded(request: request, data: data);

            if (auth != null)
                await auth.Prepare(request);

            var response = await SendRequestAsync(request);
            return response;
        }

        async Task<HttpResponseMessage> SendPostAsync(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, object> files = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            appendHeaders(request: request, headers: headers);


            if (files != null)
                prepareMultipartContent(request: request, files: files);
            else if (json != null)
                prepareJson(request: request, json: json);
            else if (data != null)
                prepareUrlEncoded(request: request, data: data);

            if (auth != null)
                await auth.Prepare(request);

            var response = await SendRequestAsync(request);
            return response;
        }

        async Task<HttpResponseMessage> SendGetAsync(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            var requestUri = await prepareUrlParameters(url, parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            appendHeaders(request: request, headers: headers);

            if (auth != null)
                await auth.Prepare(request);

            var response = await SendRequestAsync(request);
            return response;
        }

        async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            using (HttpClient httpClient = new HttpClient())
            {
            HttpResponseMessage result;
            var cancelTokenSource = new CancellationTokenSource();
            var stopwatch = new Stopwatch();
            Log.Debug("{Method} '{RequestUri}'", request.Method, request.RequestUri);

            if (ConnectivityService.IsConnected == false)
                throw new ConnectivityException();

            stopwatch.Start();
            try
            {
                    result = await httpClient.SendAsync(request, cancelTokenSource.Token);
              
            }
            catch(TaskCanceledException ex)
            {
                stopwatch.Stop();
                Log.Error(ex, "Failed to {Method} ({Milliseconds}ms) '{RequestUri}', timeout ({TotalSeconds}) exceeded.", request.Method, stopwatch.ElapsedMilliseconds, request.RequestUri, HttpClient.Timeout.TotalSeconds);
                throw new TimeoutException();
            }
            catch(HttpRequestException ex)
            {
                stopwatch.Stop();
                Log.Error(ex, "Failed to {Method} ({Milliseconds}ms) '{RequestUri}'", request.Method, stopwatch.ElapsedMilliseconds, request.RequestUri);
                throw new RequestException(ex.Message);
            }
            catch(Exception ex)
            {
                stopwatch.Stop();
                Log.Error(ex, "Failed to {Method} ({Milliseconds}ms) '{RequestUri}'", request.Method, stopwatch.ElapsedMilliseconds, request.RequestUri);
                throw new RequestException(ex.Message);
            }

            stopwatch.Stop();
            if (result.IsSuccessStatusCode)
            {
                Log.Debug("Successfull {Method} ({Milliseconds}ms) '{RequestUri}'", request.Method, stopwatch.ElapsedMilliseconds, request.RequestUri);
                return result;
            }

            Log.Error("Failed to {Method} ({Milliseconds}ms) '{RequestUri}': {StatusCode}", request.Method, stopwatch.ElapsedMilliseconds, request.RequestUri, result.StatusCode);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
                throw new Unauthorized();
            else if (result.StatusCode == HttpStatusCode.NotFound)
                throw new NotFound();
            else if (result.StatusCode == HttpStatusCode.Forbidden)
                throw new Forbidden();

            throw new HTTPError(status: result.StatusCode);
            }
        }

        void appendHeaders(HttpRequestMessage request, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var kvp in headers)
                {
                    request.Headers.Add(name: kvp.Key, value: kvp.Value);
                }
            }
        }

        void prepareMultipartContent(HttpRequestMessage request, Dictionary<string, object> files = null)
        {
            if (files == null)
                return;
            
            var form = new MultipartFormDataContent();

            // some older servers don't like the boundary param wrapped in ""
            // For example:
            // Content-Type: multipart/form-data; boundary="abc123"
            //
            // We strip the quotes here to ensure compatibility.
            // For example:
            // Content-Type: multipart/form-data; boundary=abc123
            //
            // https://smartfile.forumbee.com/t/k9h2pk
            // http://developers.de/blogs/damir_dobric/archive/2013/09/10/problems-with-webapi-multipart-content-upload-and-boundary-quot-quotes.aspx
            // https://stackoverflow.com/questions/21569770
            //
            // Technically the quotes are allowed, and they encourage implementers to be
            // sure they support them: See the WARNING TO IMPLEMENTORS here:
            // https://tools.ietf.org/html/rfc2046#section-5.1.1
            //
            // `HttpClient` uses a Guid by default for the boundary
            // which will never contain special characters requireing the value
            // to be quoted, so we are safe to remove them here as the spec also calls
            // out they it does not have to be quoted as long as there are no special
            // characters that would cause a parser issues.
            var boundary = form.Headers.ContentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
            boundary.Value = boundary.Value.Replace("\"", string.Empty);

            foreach (var key in files.Keys)
            {
                if (files[key] is string value)
                    form.Add(new StringContent(value), $"\"{key}\"");
                else if (files[key] is HttpFile file)
                {
                    if (file.Name == null)
                        file.Name = key;
                    form.Add(file.Content);
                }
                else
                    form.Add(new StringContent(files[key].ToString()), key);
            }

            request.Content = form;
        }

        void prepareJson(HttpRequestMessage request, IDictionary json = null)
        {
            if (json == null)
                return;

            var encoded = JsonConvert.SerializeObject(json);
            var content = new StringContent(encoded, Encoding.UTF8, "application/json");
            
            request.Content = content;
        }

        void prepareUrlEncoded(HttpRequestMessage request, Dictionary<string, string> data = null)
        {
            if (data == null)
                return;
            
            var content = new FormUrlEncodedContent(data);
            request.Content = content;
        }

        async Task<string> prepareUrlParameters(string url, Dictionary<string, string> parameters)
        {
            string requestUri;

            if (parameters != null)
            {
                using (var encodedParams = new FormUrlEncodedContent(parameters))
                {
                    var value = await encodedParams.ReadAsStringAsync();
                    requestUri = string.Format("{0}?{1}", url, value);
                }
            }
            else
            {
                requestUri = url;
            }

            return requestUri;
        }
    }
}
