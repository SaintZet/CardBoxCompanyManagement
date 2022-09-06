﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace CardBoxCompanyManagement.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var destination = ((Hyperlink)e.OriginalSource).NavigateUri;
            Trace.WriteLine("Browsing to " + destination);

            using (Process browser = new Process())
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
}