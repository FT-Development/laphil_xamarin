using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace LAPhil.Events
{
    public class Program
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("extra_message")]
        public string ExtraMessage { get; set; }

        [JsonProperty("extra_message_url")]
        public string ExtraMessageUrl { get; set; }

        [JsonProperty("producer_name")]
        public string ProducerName { get; set; }

        [JsonProperty("categories")]
        public List<string> categories { get; set; }

        public Program()
        {
        }
    }
}
