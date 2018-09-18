using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using LAPhil.User.Json;


namespace LAPhil.User.Tests.Models
{
    public class NullableModel
    {
        [JsonConverter(typeof(BlocksOfficeNullable))]
        public string String { get; set; }

        [JsonConverter(typeof(BlocksOfficeNullable))]
        public DateTimeOffset DateTime { get; set; }

        [JsonConverter(typeof(BlocksOfficeNullable))]
        public bool IsBoolean { get; set; }

        [JsonConverter(typeof(BlocksOfficeNullable))]
        public int Number { get; set; }

        [JsonConverter(typeof(BlocksOfficeNullable))]
        public List<string> StringList { get; set; }

        public NullableModel()
        {
        }
    }
}
