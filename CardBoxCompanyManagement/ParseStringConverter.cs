using Newtonsoft.Json;
using System;

namespace CardBoxCompanyManagement
{
    internal class ParseStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString);
        }

        public override object ReadJson(JsonReader reader, Type typeToConvert, object existingValue, JsonSerializer serializer)
        {
            return int.Parse((string)reader.Value);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(string);
        }
    }
}