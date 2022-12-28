using CardBox.ApiClient.Models;

namespace CardBox.ApiClient.Contracts;

public interface ICompaniesService
{
    public Task<List<Company>> GetCompaniesAsync();

    public bool Delete(Company company);

    public bool Post(Company company);

    public bool Put(Company company);
}