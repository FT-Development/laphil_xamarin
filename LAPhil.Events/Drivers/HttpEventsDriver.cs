using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Reactive.Linq;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.HTTP;


namespace LAPhil.Events
{
    public class HttpEventsDriver: IEventsDriver
    {
        Logger<HttpEventsDriver> Log = ServiceContainer.Resolve<LoggingService>().GetLogger<HttpEventsDriver>();
        HttpService Http = ServiceContainer.Resolve<HttpService>();
        public string Host { get; private set; }


        public HttpEventsDriver(string host)
        {
            Host = host;
            Log.Info("Initialized HttpEventsDriver");
        }

        public IObservable<Event[]> UpcomingEvents(int? daysFromNow)
        {
            var dateCurrent = DateTime.Now;
            var dateFuture = dateCurrent.AddDays(daysFromNow ?? 84);

            // CurrentEvents could fail during it's HTTP request
            // but the Cache result could be there. AKA a Connectivity error.
            // if that happens, the CurrentEvents Pipeline will not run
            // specifically `.Where(x => x.StartTime >= dateCurrent)`
            // so we double that up here just to be sure in the `Where`
            // part of this pipeline.

            //return CurrentEvents()
                //.Select(x => x.Where(y => y.StartTime >= dateCurrent && y.StartTime <= dateFuture))
                //.Select(x => x.ToArray());
            return CurrentEvents()
                .Select(x => x.Take(10))
                .Select(x => x.ToArray());
            
            //return CurrentEvents()
                //.Select(x => x.Where(y => y.StartTime >= dateCurrent && y.StartTime <= dateFuture))
                //.Select(x => x.ToArray());
        }

        //Harish_10M
        public IObservable<Event[]> EventsTypeList()
        {
            var url = Url(EventEndpoint.Performances);
            var dateCurrent = DateTime.Now;

            return Observable.Create<Event[]>(async (observer) =>
            {
                while (url != null)
                {
                    var result = await Http.GetResultAsync<HttpEventsResponse>(url);

                    if (result.IsFailure)
                    {
                        observer.OnError(result.Error);
                        break;
                    }

                    url = result.Value.Next;
                    foreach (Event ev in result.Value.Results)
                    {
                        ev.Program.AdjustName();
                    }
                    observer.OnNext(result.Value.Results);
                }
                observer.OnCompleted();
            })
            // EventEndpoint.Performances already returns the most recent
            // but because we could be offline for the day, 
            // ensure we only have the most recent JUST IN CASE.
            .SelectMany(x => x.ToObservable())
            .Where(x => x.StartTime >= dateCurrent )
            .Aggregate(new List<Event>(), (accum, next) =>
            {
                if(!accum.Contains(next))
                {
                    accum.Contains(next);    
                }

                //accum.Add(next);    
                return accum;
            })
            .Select(x => x.ToArray())
            .Catch(Observable.Return<Event[]>(null))
            .WithCache(key: url);
        }



        public IObservable<Event[]> CurrentEvents()
        {
            var url = Url(EventEndpoint.Performances);
            var dateCurrent = DateTime.Now.AddHours(-4);

            return Observable.Create<Event[]>(async (observer) =>
            {
                while (url != null)
                {
                    var result = await Http.GetResultAsync<HttpEventsResponse>(url);

                    if (result.IsFailure)
                    {
                        observer.OnError(result.Error);
                        break;
                    }

                    url = result.Value.Next;
                    foreach (Event ev in result.Value.Results)
                    {
                        ev.Program.AdjustName();
                    }
                    observer.OnNext(result.Value.Results);
                }
                observer.OnCompleted();
            })
            // EventEndpoint.Performances already returns the most recent
            // but because we could be offline for the day, 
            // ensure we only have the most recent JUST IN CASE.
            .SelectMany(x => x.ToObservable())
            .Where(x => x.StartTime >= dateCurrent)
            .Aggregate(new List<Event>(), (accum, next) =>
            {
                accum.Add(next);
                return accum;
            })
            .Select(x => x.ToArray())
            .Catch(Observable.Return<Event[]>(null))
            .WithCache(key: url);
        }

        public IObservable<Event> EventDetail(Event target)
        {
            return Http
                .WithCache(key: target.Url)
                .Get<Event>(url: target.Url);
        }

        public string Url(EventEndpoint endpoint)
        {
            return $"{Host}/{endpoint.Value}";
        }

        public IObservable<byte[]> GetEventImageBytes(Event target, Size size)
        {
            var url = target.ImageUrl;

            if (url == null || url == string.Empty)
                return Observable.Return(default(byte[]));
            
            return GetImageBytes(url: url, size: size);
        }

        public IObservable<byte[]> GetEventImage1x1Bytes(Event target, Size size)
        {
            var url = target.PreferredImage1x1Url;

            if (url == null || url == string.Empty)
                return Observable.Return(default(byte[]));

            return GetImageBytes(url: url, size: size);
        }

        public IObservable<byte[]> GetEventImage3x2Bytes(Event target, Size size)
        {
            var url = target.PreferredImage3x2Url;

            if (url == null || url == string.Empty)
                return Observable.Return(default(byte[]));

            return GetImageBytes(url: url, size: size);
        }

        IObservable<byte[]> GetImageBytes(string url, Size size)
        {
            var parameters = new List<string>();

            var initialPartition = url.EndsWith("/", StringComparison.InvariantCulture) ? "-/" : "/-/";

            parameters.Add($"scale_crop/{size.Width}x{size.Height}/center/");

            if (parameters.Count > 0)
                url = $"{url}{initialPartition}{string.Join("/-/", parameters)}";
            
            return Observable.FromAsync(async () =>
            {
                var response = await Http.GetAsync(url: url);

                if (response.Response.IsSuccessStatusCode == false)
                    return default(byte[]);

                var bytes = await response.ByteArrayAsync();
                return bytes;
            })
            .WithCache(key: url, continueOnCacheHit: false);
        }
    }
}
