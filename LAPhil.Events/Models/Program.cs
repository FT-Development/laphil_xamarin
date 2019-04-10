using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

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

        public void AdjustName()
        {

            Name = Name.Replace("\\", "<br/>");
            Name = replacePairs(Name, '*', "<b>", "</b>");
            Name = replacePairs(Name, '_', "<i>", "</i>");

        }

        private String replacePairs(String data, char pairCode, String replaceFirstWith, String replaceSecondWith)
        {

            String[] tokens = data.Split(pairCode);
            StringBuilder builder = new StringBuilder();
            builder.Append(tokens[0]);

            for (int i = 1; i<tokens.Length; i++)
            {
                builder.Append(i % 2 == 1 ? replaceFirstWith : replaceSecondWith);
                builder.Append(tokens[i]);
            }

            return builder.ToString();

        }
    }
}
