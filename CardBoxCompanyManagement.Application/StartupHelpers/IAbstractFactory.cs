namespace CardBoxCompanyManagement.StartupHelpers;

internal interface IAbstractFactory<T>
{
    T Create();
}