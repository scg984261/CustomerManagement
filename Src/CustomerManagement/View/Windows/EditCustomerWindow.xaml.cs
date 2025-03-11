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
        public CustomerItemViewModel SelectedCustomer { get; set; }
        private readonly string? companyName;
        private readonly string? businessContact;
        private readonly string? contactNumber;
        private readonly string? emailAddress;

        public EditCustomerWindow(CustomerItemViewModel selectedCustomer)
        {
            InitializeComponent();
            this.SelectedCustomer = selectedCustomer;
            this.companyName = selectedCustomer.CompanyName;
            this.businessContact = selectedCustomer.BusinessContact;
            this.contactNumber = selectedCustomer.ContactNumber;
            this.emailAddress = selectedCustomer.EmailAddress;
            this.DataContext = SelectedCustomer;
        }

        public void CancelButton_Click(object sender, RoutedEventArgs args)
        {
            // Reset Customer values to what they were originally.
            this.SelectedCustomer.CompanyName = this.companyName;
            this.SelectedCustomer.BusinessContact = this.businessContact;
            this.SelectedCustomer.ContactNumber = this.contactNumber;
            this.SelectedCustomer.EmailAddress = this.emailAddress;
            this.Close();
        }

        public void SaveButton_Click(object sender, RoutedEventArgs args)
        {
            this.Close();
        }
    }
}
