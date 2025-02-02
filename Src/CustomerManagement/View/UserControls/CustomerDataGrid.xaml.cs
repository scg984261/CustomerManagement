using System.Collections.ObjectModel;
using System.Windows.Controls;
using CDB;
using CDB.Model;

namespace CustomerManagement.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomerDataGrid.xaml
    /// </summary>
    public partial class CustomerDataGrid : UserControl
    {
        public ObservableCollection<Customer>? Customers { get; set; }
        private DataWrapper wrapper;

        public CustomerDataGrid()
        {
            DataContext = this;
            this.wrapper = new DataWrapper();
            this.Customers = new ObservableCollection<Customer>(wrapper.SelectAllCustomers());
            InitializeComponent();
        }

        public void Window_Loaded(object sender, EventArgs e)
        {
            this.CustomerTable.ItemsSource = this.Customers;
        }

        private void CustomerTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs rowEventArgs)
        {
            Customer customerToUpdate = (Customer) this.CustomerTable.SelectedValue;
        }

        // Todo: use observable collection to refresh the UI without needing to re-query the database.
        public void Refresh()
        {
            this.Customers = new ObservableCollection<Customer>(wrapper.SelectAllCustomers());            
            this.CustomerTable.ItemsSource = this.Customers;
        }
    }
}
