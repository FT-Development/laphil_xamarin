using System;
using Newtonsoft.Json;


namespace LAPhil.Events.Json
{
    public class TimeSpanConverter: JsonConverter
    {
        
        public TimeSpanConverter(params Type[] types)
        {
        }

        public TimeSpanConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Integer){
                var value = Convert.ToInt32((Int64)reader.Value);
                var result = new TimeSpan(hours: 0, minutes: 0, seconds: value);
                return result;
            }

            return new TimeSpan();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
