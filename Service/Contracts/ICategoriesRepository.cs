namespace CardBoxCompanyManagement.Infrastructure
{
    public interface ICategoriesRepository
    {
        public Dictionary<string, string> GetAll();
    }
}