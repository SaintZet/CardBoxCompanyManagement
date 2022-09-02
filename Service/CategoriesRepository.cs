using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CardBoxCompanyManagement
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private Dictionary<string, string> Load()
        {
            HttpRequest httpRequest = new("https://microinvest.cardbox.bg/categories/");

            var response = httpRequest.Get();

            var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(response);

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var dictionary in list)
            {
                dictionary.ToList().ForEach(x => result.Add(x.Key, x.Value));
            }

            return result;
        }
    }
}