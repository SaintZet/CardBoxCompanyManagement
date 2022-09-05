using CardBoxCompanyManagement.Infrastructure.Convertors;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class Company
{
    public Company(int ID, string Name, Category category, string summary)
    {
        this.ID = ID;
        this.Name = Name;
        Category = category;
        Summary = summary;
    }

    [JsonProperty("bulstat")]
    [JsonConverter(typeof(ParseStringConverter))]
    public int ID { get; set; }

    [JsonProperty("title")]
    public string Name { get; set; }

    [JsonProperty("category_id")]
    [JsonConverter(typeof(StringToCategoryConverter))]
    public Category Category { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("image")]
    public Uri? Image { get; set; } = null;
}