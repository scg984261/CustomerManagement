using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using CDB.Model;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.View.Windows;
using Microsoft.Win32;

namespace CustomerManagement.ViewModel
{
    public class CustomersViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersViewModel));
        private readonly ICustomerDataProvider customerDataProvider;
        public ObservableCollection<CustomerItemViewModel> Customers { get; } = new ObservableCollection<CustomerItemViewModel>();

        private CustomerItemViewModel? selectedCustomer;
        public CustomerItemViewModel? SelectedCustomer
        {
            get
            {
                return this.selectedCustomer;
            }
            set
            {
                this.selectedCustomer = value;
                this.NotifyPropertyChanged();
                this.EditCustomerCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddCustomerCommand { get; }
        public DelegateCommand EditCustomerCommand { get;  }

        public CustomersViewModel(ICustomerDataProvider customerDataProvider)
        {
            this.customerDataProvider = customerDataProvider;
            this.AddCustomerCommand = new DelegateCommand(this.AddCustomer);
            this.EditCustomerCommand = new DelegateCommand(this.EditCustomer, this.CanEdit);
        }

        public async Task LoadAsync()
        {
            if (this.Customers.Any())
            {
                return;
            }

            var customers = await this.customerDataProvider.GetAllAsync();

            if (customers != null)
            {
                foreach (Customer customer in customers)
                {
                    this.Customers.Add(new CustomerItemViewModel(customer));
                }
            }
        }

        public void AddCustomer(object? parameter)
        {
            NewCustomerWindow window = new NewCustomerWindow();
            window.ShowDialog();
            
            if (window.SaveCustomerOnClose)
            {
                string? companyName = window.CompanyName;
                string? businessContact = window.BusinessContact;
                string? contactNumber = window.ContactNumber;
                string? emailAddress = window.EmailAddress;

                if (companyName is not null && businessContact is not null && contactNumber is not null && emailAddress is not null)
                {
                    Customer customer = new Customer(10, companyName, businessContact, emailAddress, contactNumber, DateTime.Now, DateTime.Now);
                    CustomerItemViewModel viewModel = new CustomerItemViewModel(customer);
                    this.Customers.Add(viewModel);
                    log.Info($"New customer with ID {customer.Id} added to the customer grid.");
                }
                else
                {
                    log.Info("Request to add new customer was cancelled.");
                }
            }
        }

        public bool CanEdit(object? parameter)
        {
            return this.SelectedCustomer != null;
        }

        public void EditCustomer(object? parameter)
        {
            EditCustomerWindow editCustomerWindow = new EditCustomerWindow(this.SelectedCustomer);
            editCustomerWindow.ShowDialog();

            log.Info($"Customer with ID {this.SelectedCustomer.Id} successfully updated.");
        }
    }
}
