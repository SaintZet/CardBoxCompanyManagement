using CardBox.ApiClient.Models;
using CardBox.CompanyManagement.DataProvider;
using CardBox.CompanyManagement.Services;
using CardBox.CompanyManagement.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace CardBox.CompanyManagement.ViewModels;

internal class MainViewModel : BaseViewModel
{
    private static readonly int[] pageSizes = { 10, 15, 30, 50 };
    private readonly Paging<CompanyWrapper> _pagingManager = new();
    private readonly IWindowService _windowService;
    private readonly ICompanyDataProvider _companiesProvider;
    private int _selectedPageSize = pageSizes[0];
    private List<CompanyWrapper> _allCompanies;
    private List<CompanyWrapper> _filteredCompanies;
    private List<CompanyWrapper> _pagedCompanies;
    private string _search = string.Empty;

    public MainViewModel(ICompanyDataProvider companiesProvider, IWindowService windowService)
    {
        _companiesProvider = companiesProvider;
        _windowService = windowService;

        _allCompanies = _companiesProvider.GetCompanyWrappers();
        _filteredCompanies = _allCompanies;

        _pagingManager.PageIndex = 0;
        _pagedCompanies = _pagingManager.SetPaging(_filteredCompanies, SelectedPageSize);
    }

    public static int[] PageSizes => pageSizes;
    public List<CompanyWrapper> AllCompanies
    {
        get => _allCompanies!;
        set
        {
            _allCompanies = value;
            _filteredCompanies = Filtered(_allCompanies);

            PagedCompaniesRefresh();
            OnPropertyChanged(nameof(RecordsCount));
        }
    }
    public List<CompanyWrapper> PagedCompanies
    {
        get => _pagedCompanies;
        set
        {
            if (_pagedCompanies != value)
            {
                _pagedCompanies = value;
                OnPropertyChanged(nameof(PagedCompanies));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
    }
    public int CurrentPage => _pagingManager.PageIndex + 1;
    public int RecordsCount => _allCompanies.Count;
    public int SelectedPageSize
    {
        get => _selectedPageSize;
        set
        {
            if (_selectedPageSize != value)
            {
                _selectedPageSize = value;
                PagedCompaniesRefresh();
            }
        }
    }
    public CompanyWrapper? SelectedCompany { get; set; }
    public string SearchCriteria
    {
        get => _search;
        set
        {
            _search = value;
            _filteredCompanies = value != string.Empty ? Filtered(_allCompanies) : _allCompanies;

            PagedCompaniesRefresh();
        }
    }

    public ICommand AddCompanyCommand => new RelayCommand(execute: AddCompanyWindow);
    public ICommand DeleteCompanyCommand => new RelayCommand(execute: DeleteCompanyWindow, _ => SelectedCompany != null);
    public ICommand EditCompanyCommand => new RelayCommand(execute: EditCompanyWindow, _ => SelectedCompany != null);
    public ICommand NextPage => new RelayCommand(execute: (_) => PagedCompanies = _pagingManager.Next(_filteredCompanies, _selectedPageSize));
    public ICommand PreviousPage => new RelayCommand(execute: (_) => PagedCompanies = _pagingManager.Previous(_filteredCompanies, _selectedPageSize));
    public ICommand FirstPage => new RelayCommand(execute: (_) => PagedCompanies = _pagingManager.First(_filteredCompanies, _selectedPageSize));
    public ICommand LastPage => new RelayCommand(execute: (_) => PagedCompanies = _pagingManager.Last(_filteredCompanies, _selectedPageSize));

    private void AddCompanyWindow(object _)
    {
        Company company = new();
        if (_windowService.AddWindow(company) == true)
        {
            _companiesProvider.Post(company);
            AllCompanies = _companiesProvider.GetCompanyWrappers();
        }
    }

    private void DeleteCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!.Company);
        if (_windowService.DeleteWindow(company) == true)
        {
            _companiesProvider.Delete(company);
            AllCompanies = _companiesProvider.GetCompanyWrappers();
        }
    }

    private void EditCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!.Company);
        if (_windowService.EditWindow(company) == true)
        {
            _companiesProvider.Put(company);
            AllCompanies = _companiesProvider.GetCompanyWrappers();
        }
    }

    private List<CompanyWrapper> Filtered(List<CompanyWrapper> companies)
    {
        return companies.FindAll(c => c.Company.Name.Contains(_search) || c.Company.ID.Contains(_search)).ToList();
    }

    private void PagedCompaniesRefresh()
    {
        _pagingManager.PageIndex = 0;
        PagedCompanies = _pagingManager.First(_filteredCompanies, _selectedPageSize);
    }
}