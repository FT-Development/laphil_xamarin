using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using LAPhil.Events.Json;

namespace LAPhil.Events
{
    public class EventComparer : IEqualityComparer<Event>
    {
        public bool Equals(Event x, Event y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Event obj)
        {
            return obj.GetHashCode();
        }
    }


    public class Event : IEquatable<Event>
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("start_time")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("program")]
        public Program Program { get; set; }

        [JsonProperty("image")]
        public string ImageUrl { get; set; }

        [JsonProperty("image_3x2")]
        public string Image3x2Url { get; set; }

        [JsonProperty("image_1x1")]
        public string Image1x1Url { get; set; }

        public string PreferredImage1x1Url { get => Image1x1Url ?? ImageUrl; }
        public string PreferredImage3x2Url { get => Image3x2Url ?? ImageUrl; }

        [JsonProperty("series")]
        public Series Series { get; set; }

        [JsonProperty("buy_url")]
        // can be null or empty string
        public String BuyUrl { get; set; }

        [JsonProperty("preperformance_talk_start_time")]
        [JsonConverter(typeof(TimeOnlyConverter))]
        public DateTime? PerformanceTalkStartTime { get; set; }

        [JsonProperty("calendar_flag")]
        public string CalendarFlag { get; set; }

        [JsonProperty("gate_time")]
        public DateTimeOffset? GateTime { get; set; }

        [JsonProperty("ticketing_system_type")]
        // will be a value or empty string
        public string TicketingSystemType { get; set; }

        [JsonProperty("ticketing_system_id")]
        // will be a value or empty string
        public string TicketingSystemId { get; set; }

        [JsonProperty("performers")]
        public Performer[] Performers { get; set; }

        [JsonProperty("pieces")]
        public Piece[] Pieces { get; set; }

        static private Dictionary<String, String> OverrideDetailEvents = new Dictionary<string, string> {
            {"public opening: cardiff and miller video walk", "https://www.laphil.com/about/our-venues/cardiff-and-miller-video-walk/"},
            {"celebrate la!", "https://www.laphil.com/celebratela/"},
            {"the 17th korea times music festival", "http://ktmf.koreatimes.com/"}
        };


        public Event()
        {
        }

        public bool Equals(Event other)
        {
            return Url == other.Url;
        }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }

        public bool ShouldOverrideDetails() 
        {
            string url = null;
            return OverrideDetailEvents.TryGetValue(Program.Name.ToLower(), out url);
        }

        public string GetOverrideUrl() 
        {
            string url = "";
            OverrideDetailEvents.TryGetValue(Program.Name.ToLower(), out url);
            return url;
        }

    }
}
