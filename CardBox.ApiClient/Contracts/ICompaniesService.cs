using CardBox.ApiClient.Models;

namespace CardBox.ApiClient.Services;

public interface ICompaniesService
{
    public List<Company> GetCompanies();

    public bool Delete(Company company);

    public bool Post(Company company);

    public bool Put(Company company);
}