using ExtensionMethods;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class CategoriesRepository : ICategoriesRepository
{
    private static Lazy<List<Category>> categories = new(GetCategories().ToListCategory());

    public List<Category> Categories => categories.Value;

    private static Dictionary<int, string> GetCategories()
    {
        string body = JsonConvert.SerializeObject("", Formatting.Indented);
        var response = new HttpRequestManager("https://microinvest.cardbox.bg/categories/").Request(HttpMethod.Get, body);
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var list = JsonConvert.DeserializeObject<List<Dictionary<int, string>>>(responseBody);

        return MergeDictionary(list!);
    }

    private static Dictionary<int, string> MergeDictionary(List<Dictionary<int, string>> list)
    {
        Dictionary<int, string> result = new();

        foreach (var dictionary in list)
        {
            dictionary.ToList().ForEach(x => result.Add(x.Key, x.Value));
        }

        return result;
    }
}