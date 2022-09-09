using CardBoxCompanyManagement.Infrastructure;

namespace ExtensionMethods;

public static class MyExtensions
{
    public static List<Category> ToListCategory(this Dictionary<int, string> dictionary)
    {
        return dictionary.Select(p => new Category(number: p.Key, name: p.Value)).ToList();
    }
}