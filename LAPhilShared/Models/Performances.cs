using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LAPhilShared//.Models
{
    public class Performances
    {
        //public Performances()
        //{
        //}

        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }

        public class Program
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Series
        {
            public string url { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class Result
        {
            public string url { get; set; }
            public DateTime start_time { get; set; }
            public Program program { get; set; }
            public Series series { get; set; }
            public object preperformance_talk_start_time { get; set; }
            public object calendar_flag { get; set; }
            public object gate_time { get; set; }
        }

        //public class RootObject
        //{
        //    public int count { get; set; }
        //    public string next { get; set; }
        //    public object previous { get; set; }
        //    public List<Result> results { get; set; }
        //}
    }
}
