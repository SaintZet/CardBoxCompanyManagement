using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace CardBox.CompanyManagement.View;

public partial class MainWindow : Window
{
    private bool IsControlPressed = false;

    public MainWindow(object dataContext)
    {
        InitializeComponent();

        DataContext = dataContext;
    }

    private void DataGridCell_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is DataGridCell cell && cell.IsSelected == true)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                IsControlPressed = true;
            }

            if (IsControlPressed && e.Key == Key.C)
            {
                if (cell.Content is TextBlock)
                {
                    Clipboard.SetText(((TextBlock)cell.Content).Text);
                }

                IsControlPressed = false;
                e.Handled = true;
            }
        }
    }

    private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
    {
        var destination = ((Hyperlink)e.OriginalSource).NavigateUri;
        Trace.WriteLine("Browsing to " + destination);

        using (Process browser = new())
        {
            browser.StartInfo = new ProcessStartInfo
            {
                FileName = destination.ToString(),
                UseShellExecute = true,
                ErrorDialog = true
            };
            browser.Start();
        }
    }
}