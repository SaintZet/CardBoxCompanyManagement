using System.Windows;

namespace CardBoxCompanyManagement.View
{
    /// <summary>
    /// Interaction logic for AddCompany.xaml
    /// </summary>
    public partial class AddCompanyView : Window
    {
        public AddCompanyView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}