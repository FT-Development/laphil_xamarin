using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Reactive.Concurrency;
using LAPhil.Cache;
using LAPhil.Application;


namespace LAPhil.HTTP
{
    public static class IObservableCacheExtensions
    {
        public static IObservable<byte[]> WithCache(this IObservable<byte[]> source, string key, bool continueOnCacheHit = true)
        {
            return Observable.Create<byte[]>(observer => {

                Scheduler.ScheduleAsync(Scheduler.CurrentThread, async (scheduler, cancelToken) =>
                {
                    var cacheService = ServiceContainer.Resolve<CacheService>();
                    var cached = await cacheService.GetAsync(key);

                    if (cached != null)
                    {
                        observer.OnNext(cached);

                        if (continueOnCacheHit == false)
                        {
                            observer.OnCompleted();
                            return;
                        }
                    }


                    source.Do(value =>
                    {

                        if (value != null)
                            _ = cacheService.SetAsync(key, value);

                    }).Subscribe(value =>
                    {
                        // If we have a cached result and the `value` here is `null`
                        // we don't need to do anything. The cached result will be 
                        // returned as the final result.


                        if (value != null)
                        {
                            observer.OnNext(value);
                            return;
                        }

                        // If the cache result is `null` AND the `value` here is `null`
                        // we need to send back that `null` otherwise if the call 
                        // site is using `await` they will never see anything. It will
                        // never come back.
                        if (value == null && 
                            cached == null)
                        {
                            observer.OnNext(value);
                            return;
                        }

                    }, observer.OnError, observer.OnCompleted);
                });

                return Disposable.Empty;
            });
        }

        public static IObservable<T> WithCache<T>(this IObservable<T> source, string key, bool continueOnCacheHit = true)
        {
            return Observable.Create<T>(observer => {

                Scheduler.ScheduleAsync(Scheduler.CurrentThread, async (scheduler, cancelToken) =>
                {
                    var cacheService = ServiceContainer.Resolve<CacheService>();
                    var cached = await cacheService.GetAsync<T>(key);


                    if(typeof(T).IsValueType)
                    {
                        if (EqualityComparer<T>.Default.Equals(cached, default(T)) == false)
                        {
                            observer.OnNext(cached);

                            if (continueOnCacheHit == false)
                            {
                                observer.OnCompleted();
                                return;
                            }
                        }
                    } 
                    else 
                    {
                        if(((object)cached) != null)
                        {
                            observer.OnNext(cached);

                            if (continueOnCacheHit == false)
                            {
                                observer.OnCompleted();
                                return;
                            }
                        }
                    }




                    source
                    .Do(value =>
                    {

                        if (EqualityComparer<T>.Default.Equals(value, default(T)) == false)
                            _ = cacheService.SetAsync(key, value);

                    }).Subscribe(value =>
                    {
                        // If we have a cached result and the `value` here is `null`
                        // we don't need to do anything. The cached result will be 
                        // returned as the final result.


                        if (EqualityComparer<T>.Default.Equals(value, default(T)) == false)
                        {
                            observer.OnNext(value);
                            return;
                        }

                        // If the cache result is `null` AND the `value` here is `null`
                        // we need to send back that `null` otherwise if the call 
                        // site is using `await` they will never see anything. It will
                        // never come back.
                        if (EqualityComparer<T>.Default.Equals(value, default(T)) == true &&
                          EqualityComparer<T>.Default.Equals(cached, default(T)) == true)
                        {
                            observer.OnNext(value);
                            return;
                        }
                        
                    }, observer.OnError, observer.OnCompleted);
                });

                return Disposable.Empty;
            });
        }
    }
}
