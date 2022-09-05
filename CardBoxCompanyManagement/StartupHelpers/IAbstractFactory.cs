namespace CardBoxCompanyManagement.Application.StartupHelpers
{
    internal interface IAbstractFactory<T>
    {
        T Create();
    }
}