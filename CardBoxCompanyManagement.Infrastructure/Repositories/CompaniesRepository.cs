using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class CompaniesRepository : ICompaniesRepository
{
    private static string defaultAddressToResquest = "https://microinvest.cardbox.bg/company/";
    private HttpRequestManager defaultHttpManager = new(defaultAddressToResquest);

    public List<Company> Get()
    {
        HttpRequestManager specificHttpManager = new("https://microinvest.cardbox.bg/companies/");

        string body = JsonConvert.SerializeObject("", Formatting.Indented);
        var response = specificHttpManager.Request(HttpMethod.Get, body);
        var responseBody = response.Content.ReadAsStringAsync().Result;

        List<Company> companies = new();
        foreach (var item in JsonConvert.DeserializeObject<Dictionary<string, Company>>(responseBody)!)
        {
            companies.Add(item.Value);
        }

        return companies;
    }

    public bool Delete(Company company)
    {
        string body = JsonConvert.SerializeObject(company, Formatting.Indented);
        var response = defaultHttpManager.Request(HttpMethod.Delete, body);

        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }

    public bool Post(Company company)
    {
        string body = JsonConvert.SerializeObject(company, Formatting.Indented);
        var response = defaultHttpManager.Request(HttpMethod.Post, body);

        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }

    public bool Put(Company company)
    {
        string body = JsonConvert.SerializeObject(company, Formatting.Indented);
        var response = defaultHttpManager.Request(HttpMethod.Put, body);

        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }
}