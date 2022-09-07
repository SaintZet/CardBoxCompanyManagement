using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.View;
using CardBoxCompanyManagement.ViewModels;

namespace CardBoxCompanyManagement.StartupHelpers;

internal class WindowService : IWindowService
{
    public bool? AddWindow(Company company)
    {
        AddCompanyView window = new AddCompanyView
        {
            DataContext = new CRUDCompanyViewModel(company)
        };

        return window.ShowDialog();
    }

    public bool? DeleteWindow(Company company)
    {
        DeleteCompanyView window = new DeleteCompanyView
        {
            DataContext = new CRUDCompanyViewModel(company)
        };

        return window.ShowDialog();
    }

    public bool? EditWindow(Company company)
    {
        EditCompanyView window = new()
        {
            DataContext = new CRUDCompanyViewModel(company)
        };

        return window.ShowDialog();
    }
}