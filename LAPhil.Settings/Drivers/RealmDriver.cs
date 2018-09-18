using System;
using System.Threading.Tasks;
using System.Linq;
using Realms;
using LAPhil.Application;
using LAPhil.Logging;


namespace LAPhil.Settings.Realm
{
    public class RealmDriver: ISettingsDriver
    {
        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<RealmDriver>();
        readonly RealmConfiguration Config;

        public RealmDriver(string path)
        {
            Config = new RealmConfiguration(path);
            Config.ObjectClasses = new[] { typeof(RealmAppSettings) };
        }


        public Task<AppSettings> AppSettings()
        {
            return Task.Run(() =>
            {
                AppSettings result = new AppSettings();

                using (var realm = Realms.Realm.GetInstance(Config))
                using (var trans = realm.BeginWrite())
                {
                    var obj = realm.All<RealmAppSettings>().FirstOrDefault();

                    if (obj == null)
                    {
                        return result;
                    } 

                    result.IsFirstRun = obj.IsFirstRun;
                    return result;
                }
            });
        }

        public Task Write(AppSettings settings)
        {
            return Task.Run(() =>
            {
                using (var realm = Realms.Realm.GetInstance(Config))
                using (var trans = realm.BeginWrite())
                {
                    var obj = realm.All<RealmAppSettings>().FirstOrDefault();

                    if (obj == null)
                    {
                        obj = new RealmAppSettings();
                    }

                    obj.IsFirstRun = settings.IsFirstRun;

                    realm.Add(obj, update: true);
                    trans.Commit();
                }
            });
        }
    }
}
