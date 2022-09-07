using CardBoxCompanyManagement.Infrastructure.Convertors;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class Company
{
    public Company()
    {
    }

    public Company(Company company)
    {
        ID = company.ID;
        Name = company.Name;
        Category = company.Category;
        Summary = company.Summary;
        Image = company.Image;
    }

    [JsonProperty("bulstat")]
    [JsonConverter(typeof(ParseStringConverter))]
    public int ID { get; set; }

    [JsonProperty("title")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("category_id")]
    [JsonConverter(typeof(StringToCategoryConverter))]
    public Category? Category { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonProperty("image")]
    public Uri? Image { get; set; } = null;
}