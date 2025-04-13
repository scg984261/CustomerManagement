using System.Collections.ObjectModel;
using log4net;
using CDB.Model;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.Windows;

namespace CustomerManagement.ViewModel
{
    public class CustomersViewModel : ViewModelBase
    {   
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersViewModel));
        private readonly ICustomerDataProvider customerDataProvider;
        private NavigationStore navigationStore;
        private CustomerItemViewModel? selectedCustomer;

        public ObservableCollection<CustomerItemViewModel> Customers { get; } = new ObservableCollection<CustomerItemViewModel>();

        public DelegateCommand NavigateDetailsCommand { get; }
        public DelegateCommand NavigateNewCustomerCommand { get; }

        public CustomersViewModel(NavigationStore navigationStore, ICustomerDataProvider customerDataProvider)
        {
            this.navigationStore = navigationStore;
            this.customerDataProvider = customerDataProvider;
            this.NavigateDetailsCommand = new DelegateCommand(this.NavigateToCustomerDetails, this.IsCustomerSelected);
            this.NavigateNewCustomerCommand = new DelegateCommand(this.NavigateToNewCustomer);
        }

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
                this.NavigateDetailsCommand.RaiseCanExecuteChanged();
            }
        }

        public override void Load()
        {
            try
            {
                if (this.Customers.Any())
                {
                    return;
                }

                var customers = this.customerDataProvider.GetAll();

                if (customers != null)
                {
                    foreach (Customer customer in customers)
                    {
                        this.Customers.Add(new CustomerItemViewModel(customer));
                    }

                    log.Debug($"Customers successfully loaded. {customers.Count} returned.");
                }
            }
            catch (Exception exception)
            {

            }
        }

        public bool IsCustomerSelected(object? parameter)
        {
            return this.SelectedCustomer != null;
        }

        public void NavigateToCustomerDetails(object? parameter)
        {
            if (this.selectedCustomer != null)
            {
                CustomerDetailsViewModel customerDetailsViewModel = new CustomerDetailsViewModel(this.selectedCustomer, this.navigationStore, this.customerDataProvider, new MessageBoxHelper());
                this.navigationStore.SelectedViewModel = customerDetailsViewModel;

                this.navigationStore.SelectedViewModel.Load();
            }
        }

        public void NavigateToNewCustomer(object? parameter)
        {
            this.navigationStore.SelectedViewModel = new NewCustomerViewModel(this.navigationStore, this.customerDataProvider, new MessageBoxHelper());
        }
    }
}
