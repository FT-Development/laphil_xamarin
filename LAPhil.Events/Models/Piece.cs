using System;
using Newtonsoft.Json;


namespace LAPhil.Events
{
    public class Piece
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("composer")]
        public string ComposerUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("listen_text")]
        public string ListenText { get; set; }

        [JsonProperty("listen_url")]
        public string ListenUrl { get; set; }

        [JsonProperty("composer_name")]
        public string ComposerName { get; set; }

        public Piece()
        {
        }
    }
}
