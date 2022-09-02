using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace CardBoxCompanyManagement
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ICategoriesRepository category;
        private readonly ICompaniesRepository companies;
        private string search = string.Empty;

        public MainViewModel(ICategoriesRepository category, ICompaniesRepository companies)
        {
            this.category = category;
            this.companies = companies;
            //var x = LoadCategories();

            Companies = new ObservableCollection<Company>(companies.GetAll());
            DataList.Filter = new Predicate<object>(c => Filter((Company)c));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Company> Companies { get; set; }

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

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private bool Filter(Company company)
        {
            return SearchCriteria == null
                || company.ID.ToString().IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1
                || company.Name.IndexOf(SearchCriteria, StringComparison.OrdinalIgnoreCase) != -1;
        }

        //public ICommand SearchCommand => new RelayCommand(execute: AcceptFilters, canExecute: _ => true);
        //public ICommand ResetCommand => new RelayCommand(execute: _ => DataList = new ObservableCollection<Company>(LoadCompanies()), canExecute: _ => true);
        //private void AcceptFilters(object _)
        //{
        //    DataList = new ObservableCollection<Company>(originalDataList.Where(x => x.Category == "24").ToList());
        //}
    }
}