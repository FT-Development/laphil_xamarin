#if __IOS__
using Foundation;
using System;
using System.IO;


namespace LAPhil.Platform
{
    public static class PathService
    {
        public static string CachePath
        {
            get
            {
                NSError err;

                var fileManager = new NSFileManager();
                var bundle = NSBundle.MainBundle.BundleIdentifier;
                var url = fileManager.GetUrl(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.All, null, true, out err);
                var path = Path.Combine(url.RelativePath, bundle, "caches");

                return path;
            }
        }

        public static string SettingsPath
        {
            get
            {
                NSError err;

                var fileManager = new NSFileManager();
                var bundle = NSBundle.MainBundle.BundleIdentifier;
                var url = fileManager.GetUrl(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.All, null, true, out err);
                var path = Path.Combine(url.RelativePath, bundle, "settings");

                return path;
            }
        }

        public static void CreatePath(string path)
        {
            var fileManager = new NSFileManager();
            try
            {
                fileManager.CreateDirectory(path, createIntermediates: true, attributes: null);
            }
            catch (Exception e)
            {
                // noop
            }
        }
    }
}
#endif