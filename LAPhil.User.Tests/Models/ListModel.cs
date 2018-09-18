using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using LAPhil.User.Json;

namespace LAPhil.User.Tests.Models
{
    public class ListModel
    {
        [JsonConverter(typeof(BlocksOfficeList<NullableModel>))]
        public List<NullableModel> NullableList { get; set; }

        [JsonConverter(typeof(BlocksOfficeArray<NullableModel>))]
        public NullableModel[] NullableArray { get; set; }

        public ListModel()
        {
        }
    }
}
