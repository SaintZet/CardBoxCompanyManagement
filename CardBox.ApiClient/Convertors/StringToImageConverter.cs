using CardBox.ApiClient.Models;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure.Convertors;

internal class StringToImageConverter : JsonConverter
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        string writeValue = string.Empty;

        Image image = (Image)value!;
        if (image.Base64 != null || image.Uri != null)
        {
            writeValue = image.Base64 ?? image.Uri!.ToString();
        }

        writer.WriteValue(writeValue);
    }

    public override object ReadJson(JsonReader reader, Type typeToConvert, object? existingValue, JsonSerializer serializer)
    {
        var imageUri = reader.Value!.ToString();

        if (imageUri == string.Empty)
        {
            return new Image(null, null);
        }

        return new Image(new Uri(imageUri!), null);
    }
}