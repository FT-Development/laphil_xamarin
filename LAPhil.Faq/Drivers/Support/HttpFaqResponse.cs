using System;


namespace LAPhil.Faq
{
    public class HttpFaqResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public Faq[] Results { get; set; }

        public HttpFaqResponse()
        {
        }
    }
}
