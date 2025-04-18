using CustomerManagement.Command;
using CustomerManagement.Navigation;
using CustomerManagement.Data;
using CustomerManagement.Windows;
using CDB.Model;
using log4net;
using CustomerManagement.ViewModel.ServiceViewModels;

namespace CustomerManagement.ViewModel.CustomerViewModels
{
    public class CustomerDetailsViewModel : ValidationViewModelBase
    {
        private readonly ICustomerDataProvider customerDataProvider;
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;
        private IMessageBoxHelper messageBoxHelper;
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

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }

        public static CustomersViewModel? ParentCustomersViewModel { get; set; }

        public CustomerDetailsViewModel(CustomerItemViewModel customerItemViewModel, NavigationStore navigationStore, ICustomerDataProvider customerDataProvider, IMessageBoxHelper messageBoxHelper)
        {
            this.messageBoxHelper = messageBoxHelper;
            this.customerDataProvider = customerDataProvider;

            // Save values as readonly in case user cancels.
            this.initialCompanyName = customerItemViewModel.CompanyName;
            this.initialBusinessContact = customerItemViewModel.BusinessContact;
            this.initialContactNumber = customerItemViewModel.ContactNumber;
            this.initialEmailAddress = customerItemViewModel.EmailAddress;
            this.initialIsActive = customerItemViewModel.IsActive;

            this.customerItemViewModel = customerItemViewModel;

            this.subscribedServices = new List<ServiceItemViewModel>();
            this.RecurringServices = new List<ServiceItemViewModel>();
            this.NonRecurringServices = new List<ServiceItemViewModel>();

            this.customerDataProvider.LoadSubscriptions(this.Id);
            this.subscriptions = this.customerItemViewModel.Subscriptions;
            this.PopulateServices();

            this.navigationStore = navigationStore;
            this.CancelCommand = new DelegateCommand(this.Cancel);
            this.SaveCommand = new DelegateCommand(this.SaveCustomer, this.CanSaveCustomer);
        }

        public int Id
        {
            get
            {
                return this.customerItemViewModel.Id;
            }
        }

        public string? CompanyName
        {
            get
            {
                return this.customerItemViewModel.CompanyName;
            }

            set
            {
                this.customerItemViewModel.CompanyName = value;

                if (string.IsNullOrEmpty(this.customerItemViewModel.CompanyName))
                {
                    const string errorMessage = "Company name cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.NotifyPropertyChanged();
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string? BusinessContact
        {
            get
            {
                return this.customerItemViewModel.BusinessContact;
            }
            set
            {
                this.customerItemViewModel.BusinessContact = value;

                if (string.IsNullOrEmpty(this.customerItemViewModel.BusinessContact))
                {
                    const string errorMessage = "Business contact cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.NotifyPropertyChanged();
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string? ContactNumber
        {
            get
            {
                return this.customerItemViewModel.ContactNumber;
            }
            set
            {
                this.customerItemViewModel.ContactNumber = value;

                if (string.IsNullOrEmpty(this.customerItemViewModel.ContactNumber))
                {
                    const string errorMessage = "Contact number cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.SaveCommand.RaiseCanExecuteChanged();
                this.NotifyPropertyChanged();
            }
        }
        
        public string? EmailAddress
        {
            get
            {
                return this.customerItemViewModel.EmailAddress;
            }
            set
            {
                this.customerItemViewModel.EmailAddress = value;

                if (string.IsNullOrEmpty(this.customerItemViewModel.EmailAddress))
                {
                    const string errorMessage = "Email address cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.NotifyPropertyChanged();
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsActive
        {
            get
            {
                return this.customerItemViewModel.IsActive;
            }
            set
            {
                this.customerItemViewModel.IsActive = value;
                this.NotifyPropertyChanged();
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

        public void Cancel(object? parameter)
        {
            // Restore Customer values to their originals.
            this.customerItemViewModel.CompanyName = this.initialCompanyName;
            this.customerItemViewModel.BusinessContact = this.initialBusinessContact;
            this.customerItemViewModel.ContactNumber = this.initialContactNumber;
            this.customerItemViewModel.EmailAddress = this.initialEmailAddress;
            this.customerItemViewModel.IsActive = this.initialIsActive;
            this.NavigateBack();
        }

        public void NavigateBack()
        {
            if (ParentCustomersViewModel != null)
            {
                this.navigationStore.SelectedViewModel = ParentCustomersViewModel;
                this.navigationStore.SelectedViewModel.Load();
            }
        }

        public void SaveCustomer(object? parameter)
        { 
            try
            {
                int result = customerDataProvider.UpdateCustomer(this.Id);
                this.messageBoxHelper.ShowInfoDialog($"Customer record with ID {this.Id} updated.", "Customer Updated");
                this.NavigateBack();
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception {exception.GetType().FullName} occurred attempting to update customer with ID {this.Id}. Customer values will be reset to their originals.";
                log.Error(exception);
                this.messageBoxHelper.ShowErrorDialog(exception, "Error Updating Customer");
                this.Cancel(parameter);
            }
        }

        public bool CanSaveCustomer(object? parameter)
        {
            if (this.HasErrors) return false;
            if (string.IsNullOrEmpty(this.CompanyName)) return false;
            if (string.IsNullOrEmpty(this.BusinessContact)) return false;
            if (string.IsNullOrEmpty(this.ContactNumber)) return false;
            if (string.IsNullOrEmpty(this.EmailAddress)) return false;

            return true;
        }
    }
}
