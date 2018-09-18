using System;
using Realms;


namespace LAPhil.Settings.Realm
{
    public class RealmAppSettings: RealmObject
    {
        public bool IsFirstRun { get; set; }
    }
}
