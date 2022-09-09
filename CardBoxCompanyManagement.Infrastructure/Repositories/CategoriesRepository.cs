using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class CategoriesRepository : ICategoriesRepository
{
    private static Lazy<List<Category>> categories = new(GetCategories().Select(p => new Category(number: p.Key, name: p.Value)).ToList());

    public List<Category> Categories => categories.Value;

    private static Dictionary<int, string> GetCategories()
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/categories/");

        Task<string> task = Task<string>.Factory.StartNew(() => httpRequest.Get());
        var response = task.Result;

        var list = JsonConvert.DeserializeObject<List<Dictionary<int, string>>>(response);

        Dictionary<int, string> result = new();

        foreach (var dictionary in list!)
        {
            dictionary.ToList().ForEach(x => result.Add(x.Key, x.Value));
        }

        return result;
    }
}