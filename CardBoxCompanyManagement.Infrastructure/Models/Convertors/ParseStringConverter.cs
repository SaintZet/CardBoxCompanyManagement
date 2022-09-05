using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure.Convertors;

internal class ParseStringConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        object toString = value!.ToString()!;
        writer.WriteValue(toString);
    }

    public override object ReadJson(JsonReader reader, Type typeToConvert, object? existingValue, JsonSerializer serializer)
    {
        string? value = (string)reader.Value!;
        return int.Parse(value!);
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }
}