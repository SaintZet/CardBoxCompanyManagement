namespace CardBoxCompanyManagement.Infrastructure;

public interface ICategoriesRepository
{
    public Dictionary<int, string> GetAll();
}