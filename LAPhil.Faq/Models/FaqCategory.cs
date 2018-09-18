using System;
using Newtonsoft.Json;


namespace LAPhil.Faq
{
    public class FaqCategory
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        public FaqCategory()
        {
        }
    }
}

