using CustomerManagement.Command;
using CustomerManagement.Navigation;
using CustomerManagement.Data;
using CustomerManagement.Windows;
using CustomerManagement.ViewModel.ServiceViewModels;
using CDB.Model;
using log4net;

namespace CustomerManagement.ViewModel.CustomerViewModels
{
    public class CustomerDetailsViewModel : CustomerViewModelBase
    {
        private CustomerItemViewModel customerItemViewModel;
        private List<Subscription> subscriptions;
        private List<ServiceItemViewModel> subscribedServices;

        private readonly string? initialCompanyName;
        private readonly string? initialBusinessContact;
        private readonly string? initialContactNumber;
        private readonly string? initialEmailAddress;
        private readonly bool initialIsActive;

        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersViewModel));
        private static readonly string dateTimeFormat = "dd-MMM-yyyy HH:mm:ss";

        public List<ServiceItemViewModel> RecurringServices { get; set; }
        public List<ServiceItemViewModel> NonRecurringServices { get; set; }

        public CustomerDetailsViewModel(CustomerItemViewModel customerItemViewModel, NavigationStore navigationStore, ICustomerDataProvider customerDataProvider, IMessageBoxHelper messageBoxHelper) : base(navigationStore, customerDataProvider, messageBoxHelper)
        {
            this.customerItemViewModel = customerItemViewModel;
            // Save values as readonly in case user cancels.
            this.initialCompanyName = customerItemViewModel.CompanyName;
            this.initialBusinessContact = customerItemViewModel.BusinessContact;
            this.initialContactNumber = customerItemViewModel.ContactNumber;
            this.initialEmailAddress = customerItemViewModel.EmailAddress;
            this.initialIsActive = customerItemViewModel.IsActive;

            this.CompanyName = this.customerItemViewModel.CompanyName;
            this.BusinessContact = this.customerItemViewModel.BusinessContact;
            this.ContactNumber = this.customerItemViewModel.ContactNumber;
            this.EmailAddress = this.customerItemViewModel.EmailAddress;
            this.IsActive = this.customerItemViewModel.IsActive;

            this.subscribedServices = new List<ServiceItemViewModel>();
            this.RecurringServices = new List<ServiceItemViewModel>();
            this.NonRecurringServices = new List<ServiceItemViewModel>();

            this.customerDataProvider.LoadSubscriptions(this.Id);
            this.subscriptions = this.customerItemViewModel.Subscriptions;
            this.PopulateServices();

            this.navigationStore = navigationStore;
            this.NavigateBackCommand = new DelegateCommand(this.NavigateBack);
        }

        public int Id
        {
            get
            {
                return this.customerItemViewModel.Id;
            }
        }

        public string CreatedDateTime
        {
            get
            {
                return this.customerItemViewModel.CreatedDateTime.ToString(dateTimeFormat);
            }
        }

        public DateTime LastUpdateDateTime
        {
            get
            {
                return this.customerItemViewModel.LastUpdateDateTime;
            }
            set
            {
                this.customerItemViewModel.LastUpdateDateTime = value;
                this.NotifyPropertyChanged();
            }
        }

        public string LastUpdateDateTimeFormatted
        {
            get
            {
                return this.LastUpdateDateTime.ToString(dateTimeFormat);
            }
        }

        private void PopulateServices()
        {
            foreach (Subscription subscription in this.subscriptions)
            {
                Service service = subscription.Service;
                ServiceItemViewModel serviceItemViewModel = new ServiceItemViewModel(service);
                this.subscribedServices.Add(serviceItemViewModel);
            }

            this.RecurringServices = this.subscribedServices.Where(service => service.IsRecurring).ToList();
            this.NonRecurringServices = this.subscribedServices.Where(service => !service.IsRecurring).ToList(); ;
        }

        public void Cancel()
        {
            // Restore Customer values to their originals.
            this.customerItemViewModel.CompanyName = this.initialCompanyName;
            this.customerItemViewModel.BusinessContact = this.initialBusinessContact;
            this.customerItemViewModel.ContactNumber = this.initialContactNumber;
            this.customerItemViewModel.EmailAddress = this.initialEmailAddress;
            this.customerItemViewModel.IsActive = this.initialIsActive;
            this.NavigateBack(new object());
        }

        public override void SaveCustomer(object? parameter)
        {
            try
            {
                this.customerItemViewModel.CompanyName = this.CompanyName;
                this.customerItemViewModel.BusinessContact = this.BusinessContact;
                this.customerItemViewModel.ContactNumber = this.ContactNumber;
                this.customerItemViewModel.EmailAddress = this.EmailAddress;
                this.customerItemViewModel.IsActive = this.IsActive;

                int result = customerDataProvider.UpdateCustomer(this.Id);
                this.messageBoxHelper.ShowInfoDialog($"Customer record with ID {this.Id} updated.", "Customer Updated");
                this.NavigateBack(new object());
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception {exception.GetType().FullName} occurred attempting to update customer with ID {this.Id}. Customer values will be reset to their originals.";
                log.Error(exception);
                this.messageBoxHelper.ShowErrorDialog(exception, "Error Updating Customer");
                this.Cancel();
            }
        }
    }
}
