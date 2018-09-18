using System;
using Newtonsoft.Json;


namespace LAPhil.Faq
{
    public class Faq
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("headline")]
        public FaqHeadline Headline { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }


        public Faq()
        {
        }
    }
}
