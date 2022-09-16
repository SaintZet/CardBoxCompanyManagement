using CardBox.ApiClient.Models;
using CardBox.ApiClient.Services;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure.Convertors;

internal class StringToCategoryConverter : JsonConverter
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        Category category = (Category)value!;
        writer.WriteValue(category.Number);
    }

    public override object ReadJson(JsonReader reader, Type typeToConvert, object? existingValue, JsonSerializer serializer)
    {
        int categoryNumber = (int)(long)reader.Value!;

        var repo = new CategoriesService().GetCategories();

        return repo.First(c => c.Number == categoryNumber);
    }
}