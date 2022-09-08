using CardBoxCompanyManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace CardBoxCompanyManagement.ViewModels
{
    public enum CRUDOperation
    {
        Add,
        Delete,
        Edit
    }

    internal class CRUDCompanyViewModel : INotifyPropertyChanged
    {
        private Company? company;
        private int selectedCategory;

        public CRUDCompanyViewModel(ICategoriesRepository categories)
        {
            Categories = categories.Categories;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Company Company
        {
            get => company!;
            set
            {
                company = value;
                OnPropertyChanged(nameof(Company));
            }
        }
        public int SelectedCategory
        {
            get => selectedCategory!;
            set
            {
                selectedCategory = value;
                Company.Category = Categories.First(c => c.Number == selectedCategory);
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public List<Category> Categories { get; }

        public bool ReadOnlyID { get; private set; }

        public bool ReadOnlyName { get; private set; }

        public bool IsEnabledCategory { get; private set; } = true;

        public bool ReadOnlySummary { get; private set; }

        public bool ReadOnlyImage { get; private set; }

        //TODO: Add execute if all textbox not null
        public ICommand CompleteOperationCommand => new RelayCommand(execute: CompleteOperation, _ => true);

        public CRUDCompanyViewModel Load(Company company, CRUDOperation operation)
        {
            this.company = company;
            SelectedCategory = company.Category!.Number;

            switch (operation)
            {
                case CRUDOperation.Add:
                    break;

                case CRUDOperation.Delete:
                    ReadOnlyID = true;
                    ReadOnlyName = true;
                    IsEnabledCategory = false;
                    ReadOnlySummary = true;
                    ReadOnlyImage = true;
                    break;

                case CRUDOperation.Edit:
                    ReadOnlyID = true;
                    break;

                default:
                    throw new NotImplementedException();
            }
            return this;
        }

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void CompleteOperation(object _)
        {
            var x = System.Windows.Interop.ComponentDispatcher.IsThreadModal;
        }
    }
}