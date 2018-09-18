using System;
using Newtonsoft.Json;


namespace LAPhil.Events
{
    public class Performer
    {
        public string Url { get; set; }

        [JsonProperty("display_name")]
        public string Name { get; set; }

        public string Role { get; set; }
        public string SortName { get; set; }
        public string Organization { get; set; }
        public string Title { get; set; }
        public string Website { get; set; }
        public string Bio { get; set; }

        public Performer()
        {
        }
    }
}
