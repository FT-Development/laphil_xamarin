using System;
using Newtonsoft.Json;


namespace LAPhil.Faq
{
    public class FaqHeadline
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("category")]
        public FaqCategory Category{get; set;}


        public FaqHeadline()
        {
        }
    }
}
