using CardBox.ApiClient.Constants;
using CardBox.ApiClient.Contracts;
using CardBox.ApiClient.Helpers;
using CardBox.ApiClient.Models;
using Newtonsoft.Json;

namespace CardBox.ApiClient.Services;

public class CategoriesService : ICategoriesService
{
    private static readonly Lazy<List<Category>> categories = new(RequestToGetCategories());

    public List<Category> GetCategories() => categories.Value;

    private static List<Category> RequestToGetCategories()
    {
        var body = JsonConvert.SerializeObject("", Formatting.Indented);
        var response = new HttpRequestManager(CategoryRequests.Base).Request(HttpMethod.Get, body);
        var responseBody = response.Content.ReadAsStringAsync().Result;

        //take list with dictionary becouse no have another way with responce from API.
        var listWithDictionary = JsonConvert.DeserializeObject<List<Dictionary<int, string>>>(responseBody);
        var oneDictionary = MergeDictionary(listWithDictionary!);

        return oneDictionary.ToListCategory();
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