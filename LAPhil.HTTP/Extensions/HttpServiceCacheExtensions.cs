using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using LAPhil.Application;


namespace LAPhil.HTTP
{
    public class CachedHttpService
    {
        static HttpService HttpService = ServiceContainer.Resolve<HttpService>();
        string Key;

        public CachedHttpService(string key)
        {
            Key = key;
        }


        public IObservable<T> Get<T>(string url, Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            var request = Observable.FromAsync(async () =>
            {
                var result = await HttpService.GetResultAsync<T>(url: url, parameters: parameters, headers: headers, auth: auth);

                if (result.IsSuccess)
                    return result.Value;

                return default(T);
            });

            return SendRequestObservable(request);
        }

        public IObservable<T> Post<T>(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, object> files = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            var request = Observable.FromAsync(async () =>
            {
                var result = await HttpService.PostResultAsync<T>(url: url, data: data, json: json, files: files, headers: headers, auth: auth);

                if (result.IsSuccess)
                    return result.Value;

                return default(T);
            });

            return SendRequestObservable(request);
        }

        public IObservable<T> Put<T>(string url, Dictionary<string, string> data = null, IDictionary json = null, Dictionary<string, string> headers = null, IHttpAuth auth = null)
        {
            
            var request = Observable.FromAsync(async () =>
            {
                var result = await HttpService.PutResultAsync<T>(url: url, data: data, json: json, headers: headers, auth: auth);

                if (result.IsSuccess)
                    return result.Value;

                return default(T);
            });

            return SendRequestObservable(request);
        }

        public IObservable<T> SendRequestObservable<T>(IObservable<T> request)
        {
            // All of our request objects here use Result<T> in this class
            // any exceptions will be catured by `Result<T>.IsSuccess = false`
            // and `Result<T>.Error` will be populated with the exception.
            // All of the Http verb methods here (Get, Post, Put) are set
            // to return default(T), so a value or null (in case of reference 
            // objects) will always be returned.
            return request
                .WithCache(Key);
        }
    }

    public static class HttpServiceCacheExtensions
    {
        public static CachedHttpService WithCache(this HttpService source, string key)
        {
            return new CachedHttpService(key: key);
        }
    }
}
