using CardBox.ApiClient.Models;

namespace ExtensionMethods;

internal static class MyExtensions
{
    public static List<Category> ToListCategory(this Dictionary<int, string> dictionary)
    {
        return dictionary.Select(p => new Category(number: p.Key, name: p.Value)).ToList();
    }
}