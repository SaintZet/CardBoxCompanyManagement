using CardBox.ApiClient.Models;

namespace CardBox.CompanyManagement.Services;

internal interface IWindowService
{
    public bool? AddWindow(Company company);

    public bool? DeleteWindow(Company company);

    public bool? EditWindow(Company company);
}