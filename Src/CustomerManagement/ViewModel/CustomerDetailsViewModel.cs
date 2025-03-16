using CustomerManagement.Command;
using CustomerManagement.Navigation;

namespace CustomerManagement.ViewModel
{
    public class CustomerDetailsViewModel : ValidationViewModelBase
    {
        public static CustomersViewModel? ParentCustomersViewModel { get; set; }
        private static readonly string dateTimeFormat = "yyyy-MMM-dd HH:mm:ss";
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }

        private readonly string? companyName;
        private readonly string? businessContact;
        private readonly string? contactNumber;
        private readonly string? emailAddress;

        public CustomerDetailsViewModel(CustomerItemViewModel customerItemViewModel, NavigationStore navigationStore)
        {
            // Save values as readonly in case user cancels.
            this.companyName = customerItemViewModel.CompanyName;
            this.businessContact = customerItemViewModel.BusinessContact;
            this.contactNumber = customerItemViewModel.ContactNumber;
            this.emailAddress = customerItemViewModel.EmailAddress;

            this.customerItemViewModel = customerItemViewModel;
            this.navigationStore = navigationStore;
            this.CancelCommand = new DelegateCommand(this.Cancel);
            this.SaveCommand = new DelegateCommand(this.SaveCustomer, this.CanSaveCustomer);
        }

        public int? Id
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

        public string CreatedDateTime
        {
            get
            {
                return this.customerItemViewModel.CreatedDateTime.ToString(dateTimeFormat);
            }
        }

        public string LastUpdateDateTime
        {
            get
            {
                return this.customerItemViewModel.LastUpdateDateTime.ToString(dateTimeFormat);
            }
        }

        public void Cancel(object? parameter)
        {
            // Restore Customer values  to their originals.
            this.customerItemViewModel.CompanyName = this.companyName;
            this.customerItemViewModel.BusinessContact = this.businessContact;
            this.customerItemViewModel.ContactNumber = this.contactNumber;
            this.customerItemViewModel.EmailAddress = this.emailAddress;

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
            // Add a call here to update customer in SQL Server

            this.NavigateBack();
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
