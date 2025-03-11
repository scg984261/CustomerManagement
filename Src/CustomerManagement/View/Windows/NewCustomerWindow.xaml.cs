using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CDB;
using CDB.Model;
using CustomerManagement.ViewModel;
using log4net;

namespace CustomerManagement.View.Windows
{
    public partial class NewCustomerWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NewCustomerWindow));
        public CustomerItemViewModel CustomerViewModel;

        public NewCustomerWindow()
        {
            InitializeComponent();
            Customer customer = new Customer();
            this.CustomerViewModel = new CustomerItemViewModel(customer);
            this.DataContext = this.CustomerViewModel;
            log.Debug("New Customer Window successfully initialised.");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
