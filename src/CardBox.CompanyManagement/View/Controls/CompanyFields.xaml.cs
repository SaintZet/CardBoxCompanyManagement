using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardBox.CompanyManagement.View.UserControls
{
    public partial class CompanyFields : UserControl
    {
        public CompanyFields()
        {
            InitializeComponent();
        }

        private void Bulstat_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Bulstat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}