using System.Windows;

namespace CardBoxCompanyManagement.View
{
    public partial class EditCompanyView : Window
    {
        public EditCompanyView()
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