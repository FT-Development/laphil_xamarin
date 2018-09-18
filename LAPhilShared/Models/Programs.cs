using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LAPhilShared//.Models
{
    public class Programs
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }

        //public Programs()
        //{
        //}

        public class Performance
        {
            public string url { get; set; }
            public DateTime start_time { get; set; }
            public object calendar_flag { get; set; }
            public object preperformance_talk_start_time { get; set; }
        }

        public class Result
        {
            public string url { get; set; }
            public string name { get; set; }
            public string season { get; set; }
            public string image { get; set; }
            public string description { get; set; }
            public List<Performance> performances { get; set; }
            public object venue { get; set; }
            public bool is_lease_event { get; set; }
            public string preperformance_speaker { get; set; }
            public bool is_non_lapa { get; set; }
            public bool can_create_own_series { get; set; }
        }

        //public class RootObject
        //{
        //    public int count { get; set; }
        //    public object next { get; set; }
        //    public object previous { get; set; }
        //    public List<Result> results { get; set; }
        //}
    }
}
