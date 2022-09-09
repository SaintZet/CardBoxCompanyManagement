using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.View;
using CardBoxCompanyManagement.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CardBoxCompanyManagement.Services;

internal class WindowService : IWindowService
{
    public bool? AddWindow(Company company)
    {
        AddCompanyView window = new()
        {
            DataContext = App.AppHost!.Services.GetRequiredService<CRUDCompanyViewModel>().Load(company, CRUDOperation.Add)
        };

        return window.ShowDialog();
    }

    public bool? DeleteWindow(Company company)
    {
        DeleteCompanyView window = new()
        {
            DataContext = App.AppHost!.Services.GetRequiredService<CRUDCompanyViewModel>().Load(company, CRUDOperation.Delete)
        };

        return window.ShowDialog();
    }

    public bool? EditWindow(Company company)
    {
        EditCompanyView window = new()
        {
            DataContext = App.AppHost!.Services.GetRequiredService<CRUDCompanyViewModel>().Load(company, CRUDOperation.Edit)
        };

        return window.ShowDialog();
    }
}