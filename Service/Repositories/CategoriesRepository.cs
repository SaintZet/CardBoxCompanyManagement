using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class CategoriesRepository : ICategoriesRepository
{
    //TODO: Add lazy initialization
    public Dictionary<int, string> GetAll()
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/categories/");

        var response = httpRequest.Get();

        var list = JsonConvert.DeserializeObject<List<Dictionary<int, string>>>(response);

        Dictionary<int, string> result = new();

        foreach (var dictionary in list!)
        {
            dictionary.ToList().ForEach(x => result.Add(x.Key, x.Value));
        }

        return result;
    }
}