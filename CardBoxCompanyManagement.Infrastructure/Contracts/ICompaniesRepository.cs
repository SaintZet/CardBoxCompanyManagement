namespace CardBoxCompanyManagement.Infrastructure;

public interface ICompaniesRepository
{
    public List<Company> Get();

    public bool Post(Company company);

    public bool Delete(Company company);

    public bool Put(Company company);
}