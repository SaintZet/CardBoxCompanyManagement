using CardBoxCompanyManagement.Infrastructure;
using System;
using System.ComponentModel;

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
        private Company company;

        public CRUDCompanyViewModel(Company company, CRUDOperation operation)
        {
            this.company = company;

            switch (operation)
            {
                case CRUDOperation.Add:
                    break;

                case CRUDOperation.Delete:
                    ReadOnlyID = true;
                    ReadOnlyName = true;
                    ReadOnlyCategory = true;
                    ReadOnlySummary = true;
                    ReadOnlyImage = true;
                    break;

                case CRUDOperation.Edit:
                    ReadOnlyID = true;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Company Company
        {
            get => company;
            set
            {
                company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        public bool ReadOnlyID { get; private set; }
        public bool ReadOnlyName { get; private set; }
        public bool ReadOnlyCategory { get; private set; }
        public bool ReadOnlySummary { get; private set; }
        public bool ReadOnlyImage { get; private set; }

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}