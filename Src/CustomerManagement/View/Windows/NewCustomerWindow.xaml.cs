using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CDB;
using CDB.Model;
using log4net;

namespace CustomerManagement.View.Windows
{
    /// <summary>
    /// Interaction logic for NewCustomer.xaml
    /// </summary>
    public partial class NewCustomerWindow : Window, INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NewCustomerWindow));
        private DataWrapper dataWrapper;

        private string companyName;

        public string CompanyName
        {
            get
            {
                return this.companyName;
            }
            set
            {
                this.companyName = value;
                this.OnPropertyChanged();
            }
        }

        private string businessContact;

        public string BusinessContact
        {
            get
            {
                return this.businessContact;
            }
            set
            {
                this.businessContact = value;
                this.OnPropertyChanged();
            }
        }

        private string contactNumber;

        public string ContactNumber
        {
            get
            {
                return this.contactNumber;
            }
            set
            {
                this.contactNumber = value;
                this.OnPropertyChanged();
            }
        }

        private string emailAddress;

        public string EmailAddress
        {
            get
            {
                return this.emailAddress;
            }
            set
            {
                this.emailAddress = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public NewCustomerWindow()
        {
            InitializeComponent();
            DataContext = this;
            this.dataWrapper = new DataWrapper();
            this.CompanyName = string.Empty;
            this.BusinessContact = string.Empty;
            this.ContactNumber = string.Empty;
            this.EmailAddress = string.Empty;
            log.Debug("New Customer Window successfully initialised.");
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Customer customer = this.dataWrapper.InsertNewCustomer(this.CompanyName, this.BusinessContact, this.EmailAddress, this.ContactNumber);
            // MessageBoxResult result = MessageBox.Show($"New Customer inserted with ID {customer.Id}", "New Customer Inserted", MessageBoxButton.OK, MessageBoxImage.Information);
            // Could be a good idea to amalgamate the buttons with the data grid.
            // Add the newly inserted customer to the DataGrid observable collection.
            // this.dataGrid.Customers.Add(customer);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
