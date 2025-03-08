using CustomerManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomerManagement.View.Windows
{
    public partial class EditCustomerWindow : Window
    {
        private string? companyName;

        public string? CompanyName
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

        private string? businessContact;

        public string? BusinessContact
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

        private string? contactNumber;

        public string? ContactNumber
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

        private string? emailAddress;

        public string? EmailAddress
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

        public EditCustomerWindow(CustomerItemViewModel selectedCustomer)
        {
            InitializeComponent();
            this.DataContext = this;
            this.CompanyName = selectedCustomer.CompanyName;
            this.BusinessContact = selectedCustomer.BusinessContact;
            this.ContactNumber = selectedCustomer.ContactNumber;
            this.EmailAddress = selectedCustomer.EmailAddress;
        }

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CancelButton_Click(object sender, RoutedEventArgs args)
        {

        }

        public void SaveButton_Click(object sender, RoutedEventArgs args)
        {

        }
    }
}
