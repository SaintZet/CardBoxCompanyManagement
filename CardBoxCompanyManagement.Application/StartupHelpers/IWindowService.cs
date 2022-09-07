using CardBoxCompanyManagement.Infrastructure;

namespace CardBoxCompanyManagement.StartupHelpers;

internal interface IWindowService
{
    public bool? AddWindow(Company company);

    public bool? DeleteWindow(Company company);

    public bool? EditWindow(Company company);
}