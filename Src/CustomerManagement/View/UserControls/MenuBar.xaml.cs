using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CustomerManagement.DataLoader;

namespace CustomerManagement.View.UserControls
{
    public partial class MenuBar : UserControl
    {
        private CsvDataLoader dataLoader = new CsvDataLoader();

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

        private void MenuItemOpen_Click(object sender, System.Windows.RoutedEventArgs e)
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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\Users\Downloads";
            bool? fileSelected = dialog.ShowDialog();

            if (fileSelected == true)
            {
                string filePath = dialog.FileName;
                this.dataLoader.LoadCustomersFromFile(filePath);
            }
        }

        private void NewCustomerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
