using System;
using Newtonsoft.Json;

namespace LAPhil.Events
{
    public class Series
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("web_url")]
        public string WebUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public Series()
        {
        }
    }
}
