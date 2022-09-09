using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class CompaniesRepository : ICompaniesRepository
{
    public List<Company> Get()
    {
        List<Company> companies = new();

        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/companies/");
        var response = httpRequest.Get();

        foreach (var item in JsonConvert.DeserializeObject<Dictionary<string, Company>>(response)!)
        {
            companies.Add(item.Value);
        }

        return companies;
    }

    public void Delete(Company company)
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/company/");

        httpRequest.Put(company);
    }

    public void Post(Company company)
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/company/");

        httpRequest.Put(company);
    }

    public void Put(Company company)
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/company/");

        httpRequest.Put(company);
    }
}