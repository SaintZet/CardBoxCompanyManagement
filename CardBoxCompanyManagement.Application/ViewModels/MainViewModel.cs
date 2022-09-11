using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace CardBoxCompanyManagement.ViewModels;

internal class MainViewModel : INotifyPropertyChanged
{
    private static Paging PagedTable = new();
    private readonly ICompaniesRepository companies;
    private readonly IWindowService windowService;
    private List<int> pageSize = new List<int> { 2, 10, 15, 30, 50 };
    private string search = string.Empty;
    private Company? selectedItem;
    private List<Company> companiesView;

    private DataTable dataList;
    private int selectedPageSize;

    private int recordsCount;

    public MainViewModel(ICompaniesRepository companies, IWindowService windowService)
    {
        this.companies = companies;
        this.windowService = windowService;

        companiesView = companies.Get();
        selectedPageSize = pageSize[0];
        for (int i = 0; i < 100; i++)
        {
            companiesView.Add(new Company(companiesView[0]));
        }

        PagedTable.PageIndex = 1;
        dataList = PagedTable.SetPaging(companiesView, SelectedPageSize);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public DataTable DataList
    {
        get => dataList;
        set
        {
            dataList = value;
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged();
        }
    }

    //public ICollectionView DataList
    //{
    //    get => CollectionViewSource.GetDefaultView(CompaniesView);
    //}
    public int RecordsCount
    {
        get => DataList.Rows.Count;//DataList.Cast<object>().Count().ToString();
    }

    public int CurrentPage
    {
        get => PagedTable.PageIndex + 1;
    }

    public int SelectedPageSize
    {
        get { return selectedPageSize; }
        set
        {
            if (selectedPageSize != value)
            {
                selectedPageSize = value;
                DataList = PagedTable.First(companiesView, selectedPageSize);
                OnPropertyChanged(nameof(SelectedPageSize));
            }
        }
    }
    public List<int> NumberOfRecords => pageSize;
    public List<Company> CompaniesView
    {
        get { return companiesView!; }
        set
        {
            companiesView = value;
            OnPropertyChanged(nameof(CompaniesView));

            //DataList.Filter = new Predicate<object>(c => Filter((Company)c));
            //OnPropertyChanged(nameof(DataList));
        }
    }

    public Company? SelectedCompany
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            OnPropertyChanged();
        }
    }

    //public string SearchCriteria
    //{
    //    get { return search; }
    //    set
    //    {
    //        search = value;
    //        OnPropertyChanged(nameof(SearchCriteria));

    //        DataList.Refresh();
    //        OnPropertyChanged(nameof(RecordsCount));
    //    }
    //}

    public ICommand AddCompanyCommand => new RelayCommand(execute: AddCompanyWindow);
    public ICommand DeleteCompanyCommand => new RelayCommand(execute: DeleteCompanyWindow, _ => SelectedCompany != null);
    public ICommand EditCompanyCommand => new RelayCommand(execute: EditCompanyWindow, _ => SelectedCompany != null);

    public ICommand NextPage => new RelayCommand(execute: (_) => DataList = PagedTable.Next(companiesView, SelectedPageSize));

    public ICommand PreviousPage => new RelayCommand(execute: (_) => DataList = PagedTable.Previous(companiesView, SelectedPageSize));

    public ICommand FirstPage => new RelayCommand(execute: (_) => DataList = PagedTable.First(companiesView, SelectedPageSize));

    public ICommand LastPage => new RelayCommand(execute: (_) => DataList = PagedTable.Last(companiesView, SelectedPageSize));

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private void AddCompanyWindow(object _)
    {
        Company company = new();
        if (windowService.AddWindow(company) == true)
        {
            companies.Post(company);
            CompaniesView = companies.Get();
        }
    }

    private void DeleteCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!);
        if (windowService.DeleteWindow(company) == true)
        {
            companies.Delete(company);
            CompaniesView = companies.Get();
        }
    }

    private void EditCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!);
        if (windowService.EditWindow(company) == true)
        {
            companies.Put(company);
            CompaniesView = companies.Get();
        }
    }

    //private bool Filter(Company company)
    //{
    //    return SearchCriteria == null
    //        || company.ID.ToString().IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1
    //        || company.Name.IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1;
    //}
}