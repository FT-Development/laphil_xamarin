using System;
using System.IO;
using System.Reflection;



namespace LAPhil.User.Tests
{
    public static class ResourceManager
    {
        public static string ResourceFolder
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Resources");
            }
        }

        public static string FileString(string resourceName)
        {
            var path = Path.Combine(ResourceFolder, resourceName);
            return File.ReadAllText(path);
        }

        public static string EmbeddedString(string resourceName)
        {
            Assembly asm = typeof(ResourceManager).Assembly;

            using (Stream stream = asm.GetManifestResourceStream("artist.json"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Error obtaining resource [{resourceName}]");
                }
            }
        }
    }
}
