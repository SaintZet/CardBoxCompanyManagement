using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

public class CompaniesRepository : ICompaniesRepository
{
    public List<Company> Get()
    {
        List<Company> companies = new();

        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/companies/");

        Task<string> task = Task<string>.Factory.StartNew(() => httpRequest.Get());
        var response = task.Result;

        foreach (var item in JsonConvert.DeserializeObject<Dictionary<string, Company>>(response)!)
        {
            companies.Add(item.Value);
        }

        return companies;
    }

    public bool Post(Company company)
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/company/");

        Task<bool> task = Task<bool>.Factory.StartNew(() => httpRequest.Post(company));

        return task.Result;
    }

    public bool Put(Company company)
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/company/");

        Task<bool> task = Task<bool>.Factory.StartNew(() => httpRequest.Put(company));

        return task.Result;
    }

    public bool Delete(Company company)
    {
        HttpRequest httpRequest = new("https://microinvest.cardbox.bg/company/");

        Task<bool> task = Task<bool>.Factory.StartNew(() => httpRequest.Delete(new { bulstat = company.ID }));

        return task.Result;
    }
}