using System;
namespace LAPhil.Events
{
    public class Singleton
    {
        private static Singleton instance;
        public Event[] events;
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
