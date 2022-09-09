namespace CardBoxCompanyManagement.Infrastructure;

public interface ICompaniesRepository
{
    public List<Company> Get();

    public void Delete(Company company);

    public void Post(Company company);

    public void Put(Company company);
}