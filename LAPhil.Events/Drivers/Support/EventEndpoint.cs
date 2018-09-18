using System;


namespace LAPhil.Events
{
    public class EventEndpoint
    {
        private EventEndpoint(string value) { Value = value; }
        public string Value { get; set; }

        public static EventEndpoint Performances { get; } = new EventEndpoint("api/performances/");
    }
}
