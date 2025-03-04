using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CustomerManagement.DataLoader;
using CustomerManagement.View.Windows;
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

        public void MenuItemLoadData_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainWindow != null)
            {
                this.mainWindow.Opacity = 0.7;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.Filter = "Excel and CSV|*.csv;*.xlsx;";
            dialog.Multiselect = false;
            bool? fileSelected = dialog.ShowDialog();

            if (fileSelected == true)
            {
                string fileName = dialog.SafeFileName;
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to load customer data from file {fileName}?", "File select", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    string filePath = dialog.FileName;
                    this.LoadCustomersFromFile(filePath);
                }
            }

            if (this.mainWindow != null)
            {
                this.mainWindow.Opacity = 1.0;
            }
        }

        private void LoadCustomersFromFile(string filePath)
        {
            // List<Customer> customers = this.dataLoader.LoadCustomersFromFile(filePath);

            // foreach (Customer customer in customers)
            // {
            // }
        }

        private void NewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainWindow != null)
            {
                this.mainWindow.Opacity = 0.7;
            }

            NewCustomerWindow newCustomerWindow = new NewCustomerWindow();
            newCustomerWindow.ShowDialog();

            if (this.mainWindow != null)
            {
                this.mainWindow.Opacity = 1.0;
            }
        }
    }
}
