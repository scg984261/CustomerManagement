using System.Windows;
using CustomerManagement.Command;
using CustomerManagement.Navigation;
using CustomerManagement.Data;
using log4net;

namespace CustomerManagement.ViewModel
{
    public class CustomerDetailsViewModel : ValidationViewModelBase
    {
        public static CustomersViewModel? ParentCustomersViewModel { get; set; }
        private static readonly string dateTimeFormat = "dd-MMM-yyyy HH:mm:ss";
        private static readonly CustomerDataProvider customerDataProvider = new CustomerDataProvider();
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersViewModel));
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }

        private readonly string? initialCompanyName;
        private readonly string? initialBusinessContact;
        private readonly string? initialContactNumber;
        private readonly string? initialEmailAddress;
        private readonly bool initialIsActive;

        public CustomerDetailsViewModel(CustomerItemViewModel customerItemViewModel, NavigationStore navigationStore)
        {
            // Save values as readonly in case user cancels.
            this.initialCompanyName = customerItemViewModel.CompanyName;
            this.initialBusinessContact = customerItemViewModel.BusinessContact;
            this.initialContactNumber = customerItemViewModel.ContactNumber;
            this.initialEmailAddress = customerItemViewModel.EmailAddress;
            this.initialIsActive = customerItemViewModel.IsActive;

            this.customerItemViewModel = customerItemViewModel;
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

        public void Cancel(object? parameter)
        {
            // Restore Customer values  to their originals.
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
                customerDataProvider.UpdateCustomer(this.Id);
                MessageBox.Show($"Customer record with ID {this.Id} updated.", "Customer Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigateBack();
            }
            catch (Exception exception)
            {
                log.Error($"Exception {exception.GetType().FullName} occurred attempting to update customer with ID {this.Id}. Customer values will be reset to their originals.");
                MessageBox.Show($"Error occurred attempting to update customer.\r\nCustomer was not updated.\r\nPlease see the log file for more information.", "Error Updating Customer", MessageBoxButton.OK, MessageBoxImage.Error);
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
