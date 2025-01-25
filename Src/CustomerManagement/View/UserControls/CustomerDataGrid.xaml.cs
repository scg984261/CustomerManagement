using System.Collections.ObjectModel;
using System.Windows.Controls;
using DataModel;

namespace CustomerManagement.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomerDataGrid.xaml
    /// </summary>
    public partial class CustomerDataGrid : UserControl
    {
        public List<Customer> CustomerList { get; set; }
        public ObservableCollection<Customer>? Customers { get; set; }

        public CustomerDataGrid()
        {
            InitializeComponent();

            this.CustomerList = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    CompanyName = "Albany House Business Centres LTD",
                    BusinessContact = "Niall Gillen",
                    EmailAddress = "niallgillen@hudsonhouse.co.uk",
                    ContactNumber = "07919367388",
                    IsActive = true,
                    CreatedDateTime = DateTime.Now,
                    LastUpdateDateTime = DateTime.Now
                },
                new Customer
                {
                    Id = 2,
                    CompanyName = "Barclays Bank PLC",
                    BusinessContact = "Scott Gillen",
                    EmailAddress = "scott.gillen2@barclays.com",
                    ContactNumber = "N/A",
                    IsActive = true,
                    CreatedDateTime = DateTime.Now,
                    LastUpdateDateTime = DateTime.Now
                }
            };

            this.Customers = new ObservableCollection<Customer>(CustomerList);
        }

        public void Window_Loaded(object sender, EventArgs e)
        {
            this.CustomerTable.ItemsSource = this.Customers;
        }

        private void CustomerTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs rowEventArgs)
        {
            Customer customerToUpdate = (Customer) this.CustomerTable.SelectedValue;
            // this.DataProvider.UpdateCustomer(customerToUpdate);
        }
    }
}
