using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CustomerManagement.DataLoader;
using CDB.Model;

namespace CustomerManagement.View.UserControls
{
    public partial class MenuBar : UserControl
    {
        private CsvDataLoader dataLoader = new CsvDataLoader();
        private MainWindow? mainWindow = Application.Current.MainWindow as MainWindow;

        public MenuBar()
        {
            InitializeComponent();
        }

        private void MenuItemExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\Users\Downloads";
            bool? fileSelected = dialog.ShowDialog();

            if (fileSelected == true)
            {
                string filePath = dialog.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = filePath;
                Process.Start(startInfo);
            }
        }

        private void MenuItemLoadData_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainWindow != null)
            {
                this.mainWindow.Opacity = 0.7;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\Users\scott\OneDrive\Documents\CMS";
            dialog.Filter = "Excel and CSV|*.csv;*.xlsx;*.xls";
            dialog.Multiselect = false;
            bool? fileSelected = dialog.ShowDialog();

            if (fileSelected == true)
            {
                string fileName = dialog.SafeFileName;
                bool hasHeaders = false;
                MessageBoxResult result = MessageBox.Show($"{fileName} selected for customer data load. Does this file contain headers?", "File select", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.Cancel)
                {
                    if (this.mainWindow != null)
                    {
                        this.mainWindow.Opacity = 1.0;
                    }

                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    hasHeaders = true;
                }

                string filePath = dialog.FileName;
                List<Customer> customers = this.dataLoader.LoadCustomersFromFile(filePath, hasHeaders);

                foreach (Customer customer in customers)
                {
                    this.mainWindow?.CustomerDataGrid?.Customers?.Add(customer);
                }
            }

            if (this.mainWindow != null)
            {
                this.mainWindow.Opacity = 1.0;
            }
        }

        private void NewCustomerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

    }
}
