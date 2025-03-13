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
using CustomerManagement.Navigation;
using Microsoft.Win32;

namespace CustomerManagement.ViewModel
{
    public class CustomersViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersViewModel));
        // private NavigationStore navigationStore;
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
        public DelegateCommand EditCustomerCommand { get; }
        public NavigateCustomerCommand NavigateCommand { get; }

        public CustomersViewModel(NavigationStore navigationStore, ICustomerDataProvider customerDataProvider)
        {
            this.customerDataProvider = customerDataProvider;
            this.AddCustomerCommand = new DelegateCommand(this.AddCustomer);
            this.EditCustomerCommand = new DelegateCommand(this.EditCustomer, this.IsCustomerSelected);
            this.NavigateCommand = new NavigateCustomerCommand(navigationStore, this.IsCustomerSelected);
        }

        public override async Task LoadAsync()
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

            if (window.CustomerViewModel.AddCustomerOnClose)
            {
                this.Customers.Add(window.CustomerViewModel);
                log.Info("New customer added.");
            }
            else
            {
                log.Info("Request to add new customer was cancelled.");
            }
        }

        public bool IsCustomerSelected(object? parameter)
        {
            return this.SelectedCustomer != null;
        }

        public void EditCustomer(object? parameter)
        {
            EditCustomerWindow editCustomerWindow = new EditCustomerWindow(this.SelectedCustomer);
            editCustomerWindow.ShowDialog();

            log.Info($"Customer with ID {this.SelectedCustomer.Id} successfully updated.");
        }

        public void NavigateToServices(object? parameter)
        {
            // this.navigationStore.SelectedViewModel = this.navigationStore.ServicesViewModel;
        }
    }
}
