using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure.Models.Convertors;

internal class StringToImageConverter : JsonConverter
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        Image image = (Image)value!;
        var writeValue = image.Base64 ?? image.Uri!.ToString();
        writer.WriteValue(writeValue);
    }

    public override object ReadJson(JsonReader reader, Type typeToConvert, object? existingValue, JsonSerializer serializer)
    {
        return new Image(new Uri(reader.Value!.ToString()!), null);
    }
}