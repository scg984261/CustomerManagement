using System.Collections.ObjectModel;
using log4net;
using CDB.Model;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Navigation;

namespace CustomerManagement.ViewModel
{
    public class CustomersViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersViewModel));
        private NavigationStore navigationStore;
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
                this.NavigateDetailsCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand NavigateDetailsCommand { get; }

        public CustomersViewModel(NavigationStore navigationStore, ICustomerDataProvider customerDataProvider)
        {
            this.navigationStore = navigationStore;
            this.customerDataProvider = customerDataProvider;
            this.NavigateDetailsCommand = new DelegateCommand(this.NavigateToCustomerDetails, this.IsCustomerSelected);
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

        public bool IsCustomerSelected(object? parameter)
        {
            return this.SelectedCustomer != null;
        }

        public async void NavigateToCustomerDetails(object? parameter)
        {
            if (this.selectedCustomer != null)
            {
                CustomerDetailsViewModel customerDetailsViewModel = new CustomerDetailsViewModel(this.selectedCustomer, this.navigationStore);
                this.navigationStore.SelectedViewModel = customerDetailsViewModel;

                await this.navigationStore.SelectedViewModel.LoadAsync();
            }
        }
    }
}
