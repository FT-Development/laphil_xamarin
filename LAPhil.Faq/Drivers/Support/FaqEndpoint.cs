using System;


namespace LAPhil.Faq
{
    public class FaqEndpoint
    {
        private FaqEndpoint(string value) { Value = value; }
        public string Value { get; set; }

        public static FaqEndpoint Faqs { get; } = new FaqEndpoint("api/faqs/");

    }
}
