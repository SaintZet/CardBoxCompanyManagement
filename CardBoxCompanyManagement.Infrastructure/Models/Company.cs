using CardBoxCompanyManagement.Infrastructure.Convertors;
using CardBoxCompanyManagement.Infrastructure.Models.Convertors;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class Company
{
    public Company()
    {
    }

    public Company(Company company)
    {
        Category = company.Category;
        ID = company.ID;
        Image = company.Image;
        Name = company.Name;
        Summary = company.Summary;
    }

    [JsonProperty("bulstat")]
    public string ID { get; set; } = string.Empty;

    [JsonProperty("title")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonProperty("category_id")]
    [JsonConverter(typeof(StringToCategoryConverter))]
    public Category? Category { get; set; } = new CategoriesRepository().Categories.FirstOrDefault();

    [JsonProperty("image")]
    [JsonConverter(typeof(StringToImageConverter))]
    public Image Image { get; set; } = new Image(null, null);
}