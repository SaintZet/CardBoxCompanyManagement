using System.Windows;

namespace CardBoxCompanyManagement.View
{
    public partial class DeleteCompanyView : Window
    {
        public DeleteCompanyView()
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