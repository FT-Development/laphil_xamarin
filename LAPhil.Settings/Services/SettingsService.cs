using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;


namespace LAPhil.Settings
{
    public class SettingsService
    {
        public readonly ISettingsDriver Driver;

        public SettingsService(ISettingsDriver driver)
        {
            Driver = driver;
        }

        public IObservable<AppSettings> AppSettings()
        {
            return Observable.Create<AppSettings>(observer => {

                Scheduler.ScheduleAsync(Scheduler.CurrentThread, async (scheduler, cancelToken) =>
                {
                    AppSettings settings = new AppSettings();

                    try
                    {
                        settings = await Driver.AppSettings();
                    } catch(Exception e)
                    {
                        observer.OnError(e);
                    }

                    observer.OnNext(settings);
                    observer.OnCompleted();
                });

                return Disposable.Empty;
            });
        }

        public Task Write(AppSettings settings)
        {
            return Driver.Write(settings);
        }
    }
}
