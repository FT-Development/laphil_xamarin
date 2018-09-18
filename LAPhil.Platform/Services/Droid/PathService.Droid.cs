#if __ANDROID__
using System;
using Serilog;
using System.IO;

namespace LAPhil.Platform
{
    public static class PathService
    {
        public static string CachePath
        {
            get
            {
                string url =System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var path = Path.Combine(url, "caches");
                return path;
            }
        }

        public static string SettingsPath
        {
            get
            {
                
                string url=System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var path = Path.Combine(url, "settings");
                return path;
            }
        }

        public static void CreatePath(string path)
        {

            if (!File.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
              
            }
        }
    }
}
#endif