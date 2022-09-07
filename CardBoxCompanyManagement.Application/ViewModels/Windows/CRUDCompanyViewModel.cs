using CardBoxCompanyManagement.Infrastructure;
using System.ComponentModel;

namespace CardBoxCompanyManagement.ViewModels
{
    internal class CRUDCompanyViewModel : INotifyPropertyChanged
    {
        private Company company;

        public CRUDCompanyViewModel(Company company)
        {
            this.company = company;
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

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}