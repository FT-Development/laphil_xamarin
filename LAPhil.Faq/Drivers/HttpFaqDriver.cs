using System;
using System.Linq;
using System.Reactive.Linq;
using LAPhil.Application;
using LAPhil.Logging;
using LAPhil.HTTP;


namespace LAPhil.Faq
{
    public class HttpFaqDriver: IFaqDriver
    {
        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<HttpFaqDriver>();
        HttpService Http = ServiceContainer.Resolve<HttpService>();
        public string Host { get; private set; }


        public HttpFaqDriver(string host)
        {
            Host = host;
            Log.Info("Initialized HttpHttpFaqDriver");
        }

        public IObservable<Faq[]> GetFaqs()
        {
            var url = Url(FaqEndpoint.Faqs);

            return Observable.Create<Faq[]>(async (observer) =>
            {
                while (url != null)
                {
                    var result = await Http.GetResultAsync<HttpFaqResponse>(url);

                    if (result.IsFailure)
                    {
                        observer.OnError(result.Error);
                        break;
                    }

                    url = result.Value.Next;
                    observer.OnNext(result.Value.Results);
                }

                observer.OnCompleted();

            })
            .Aggregate(new Faq[] { }, (accum, next) =>
            {
                var result = accum.Concat(next).ToArray();
                return result;
            })
            .WithCache(key: url);
        }

        public string Url(FaqEndpoint endpoint)
        {
            return $"{Host}/{endpoint.Value}";
        }

    }
}
