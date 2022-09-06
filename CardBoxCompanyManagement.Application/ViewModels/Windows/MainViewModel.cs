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

    private readonly IAbstractFactory<AddCompany> addCompany;
    private readonly IAbstractFactory<EditCompany> editCompany;

    private string search = string.Empty;
    private Company selectedCompany;

    public MainViewModel(ICategoriesRepository category, ICompaniesRepository companies, IAbstractFactory<AddCompany> addCompany, IAbstractFactory<EditCompany> editCompany)
    {
        this.category = category;
        this.companies = companies;
        this.addCompany = addCompany;
        this.editCompany = editCompany;

        Companies = companies.GetAll();

        DataList.Filter = new Predicate<object>(c => Filter((Company)c));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Company> Companies { get; set; }

    public ICollectionView DataList
    {
        get => CollectionViewSource.GetDefaultView(Companies);
    }

    public string RecordsCount
    {
        get => DataList.Cast<object>().Count().ToString();
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

    public ICommand AddCompanyCommand => new RelayCommand(execute: _ => addCompany.Create().Show());
    public ICommand EditCompanyCommand => new RelayCommand(execute: EditCompanyWindow);

    public void OnPropertyChanged(string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private void EditCompanyWindow(object obj)
    {
        if (obj != null)
        {
            editCompany.Create().Show();
        }
    }

    private bool Filter(Company company)
    {
        return SearchCriteria == null
            || company.ID.ToString().IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1
            || company.Name.IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1;
    }
}