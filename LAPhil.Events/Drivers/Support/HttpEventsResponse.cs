using System;


namespace LAPhil.Events
{
    public class HttpEventsResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public Event[] Results { get; set; }

        public HttpEventsResponse()
        {
        }
    }
}
