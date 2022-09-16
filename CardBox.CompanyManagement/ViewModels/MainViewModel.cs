using CardBox.ApiClient.Models;
using CardBox.ApiClient.Services;
using CardBox.CompanyManagement.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace CardBox.CompanyManagement.ViewModels;

internal class MainViewModel : BaseViewModel
{
    private static readonly int[] pageSizes = new int[] { 10, 15, 30, 50 };
    private readonly Paging<Company> pagingManager = new();
    private readonly ICompaniesService companies;
    private readonly IWindowService windowService;
    private int selectedPageSize = pageSizes[0];
    private List<Company> allCompanies;
    private List<Company> filteredCompanies;
    private List<Company> pagedCompanies;
    private string search = string.Empty;

    public MainViewModel(ICompaniesService companies, IWindowService windowService)
    {
        this.companies = companies;
        this.windowService = windowService;

        allCompanies = companies.GetCompanies();
        filteredCompanies = allCompanies;

        pagingManager.PageIndex = 0;
        pagedCompanies = pagingManager.SetPaging(filteredCompanies, SelectedPageSize);
    }

    public static int[] PageSizes => pageSizes;
    public List<Company> AllCompanies
    {
        get => allCompanies!;
        set
        {
            allCompanies = value;
            filteredCompanies = Filtered(allCompanies);

            PagedCompaniesRefresh();
            OnPropertyChanged(nameof(RecordsCount));
        }
    }
    public List<Company> PagedCompanies
    {
        get => pagedCompanies;
        set
        {
            if (pagedCompanies != value)
            {
                pagedCompanies = value;
                OnPropertyChanged(nameof(PagedCompanies));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
    }
    public int CurrentPage => pagingManager.PageIndex + 1;
    public int RecordsCount => allCompanies.Count;
    public int SelectedPageSize
    {
        get => selectedPageSize;
        set
        {
            if (selectedPageSize != value)
            {
                selectedPageSize = value;
                PagedCompaniesRefresh();
            }
        }
    }
    public Company? SelectedCompany { get; set; }
    public string SearchCriteria
    {
        get => search;
        set
        {
            search = value;
            filteredCompanies = value != string.Empty ? Filtered(allCompanies) : allCompanies;

            PagedCompaniesRefresh();
        }
    }

    public ICommand AddCompanyCommand => new RelayCommand(execute: AddCompanyWindow);
    public ICommand DeleteCompanyCommand => new RelayCommand(execute: DeleteCompanyWindow, _ => SelectedCompany != null);
    public ICommand EditCompanyCommand => new RelayCommand(execute: EditCompanyWindow, _ => SelectedCompany != null);
    public ICommand NextPage => new RelayCommand(execute: (_) => PagedCompanies = pagingManager.Next(filteredCompanies, selectedPageSize));
    public ICommand PreviousPage => new RelayCommand(execute: (_) => PagedCompanies = pagingManager.Previous(filteredCompanies, selectedPageSize));
    public ICommand FirstPage => new RelayCommand(execute: (_) => PagedCompanies = pagingManager.First(filteredCompanies, selectedPageSize));
    public ICommand LastPage => new RelayCommand(execute: (_) => PagedCompanies = pagingManager.Last(filteredCompanies, selectedPageSize));

    private void AddCompanyWindow(object _)
    {
        Company company = new();
        if (windowService.AddWindow(company) == true)
        {
            companies.Post(company);
            AllCompanies = companies.GetCompanies();
        }
    }

    private void DeleteCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!);
        if (windowService.DeleteWindow(company) == true)
        {
            companies.Delete(company);
            AllCompanies = companies.GetCompanies();
        }
    }

    private void EditCompanyWindow(object _)
    {
        Company company = new(SelectedCompany!);
        if (windowService.EditWindow(company) == true)
        {
            companies.Put(company);
            AllCompanies = companies.GetCompanies();
        }
    }

    private List<Company> Filtered(List<Company> companies)
    {
        return companies.FindAll(c => c.Name.Contains(search) || c.ID.Contains(search)).ToList();
    }

    private void PagedCompaniesRefresh()
    {
        pagingManager.PageIndex = 0;
        PagedCompanies = pagingManager.First(filteredCompanies, selectedPageSize);
    }
}