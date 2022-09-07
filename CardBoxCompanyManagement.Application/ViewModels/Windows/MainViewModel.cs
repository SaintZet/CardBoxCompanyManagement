using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.StartupHelpers;
using CardBoxCompanyManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace CardBoxCompanyManagement.ViewModels;

internal class MainViewModel : INotifyPropertyChanged
{
    private readonly ICategoriesRepository category;
    private readonly ICompaniesRepository companies;
    private readonly IWindowService windowService;

    private string search = string.Empty;
    private Company? selectedItem;
    private List<Company> companiesView;

    public MainViewModel(ICategoriesRepository category, ICompaniesRepository companies, IWindowService windowService)
    {
        this.category = category;
        this.companies = companies;
        this.windowService = windowService;

        companiesView = companies.GetAll();

        DataList.Filter = new Predicate<object>(c => Filter((Company)c));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICollectionView DataList
    {
        get => CollectionViewSource.GetDefaultView(CompaniesView);
    }

    public string RecordsCount
    {
        get => DataList.Cast<object>().Count().ToString();
    }

    public List<Company> CompaniesView
    {
        get { return companiesView; }
        set
        {
            companiesView = value;
            OnPropertyChanged(nameof(CompaniesView));
        }
    }

    public Company? SelectedCompany
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            OnPropertyChanged(nameof(SelectedCompany));
        }
    }

    public string SearchCriteria
    {
        get { return search; }
        set
        {
            search = value;
            OnPropertyChanged(nameof(SearchCriteria));

            DataList.Refresh();
            OnPropertyChanged(nameof(RecordsCount));
        }
    }

    public ICommand AddCompanyCommand => new RelayCommand(execute: AddCompanyWindow);
    public ICommand DeleteCompanyCommand => new RelayCommand(execute: DeleteCompanyWindow, _ => SelectedCompany != null);
    public ICommand EditCompanyCommand => new RelayCommand(execute: EditCompanyWindow, _ => SelectedCompany != null);

    public void OnPropertyChanged(string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private void AddCompanyWindow(object _)
    {
        Company company = new();
        if (windowService.AddWindow(company) == true)
        {
            //companies.Add(company);
        }
    }

    private void DeleteCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!);
        if (windowService.DeleteWindow(company) == true)
        {
            //companies.Delete();
        }
    }

    private void EditCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!);
        if (windowService.EditWindow(company) == true)
        {
            //companies.Edit();
        }
    }

    private bool Filter(Company company)
    {
        return SearchCriteria == null
            || company.ID.ToString().IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1
            || company.Name.IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1;
    }
}