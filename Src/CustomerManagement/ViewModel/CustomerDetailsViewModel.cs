using CustomerManagement.Command;
using CustomerManagement.Navigation;

namespace CustomerManagement.ViewModel
{
    public class CustomerDetailsViewModel : ValidationViewModelBase
    {
        public static CustomersViewModel? ParentCustomersViewModel { get; set; }
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;
        public NavigateCustomersViewCommand NavigateBackCommand { get; }

        public CustomerDetailsViewModel(CustomerItemViewModel customerItemViewModel, NavigationStore navigationStore)
        {
            this.customerItemViewModel = customerItemViewModel;
            this.navigationStore = navigationStore;
            this.NavigateBackCommand = new NavigateCustomersViewCommand(this.navigationStore, this.CanSaveCustomer);
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
                this.NavigateBackCommand.OnCanExecuteChanged();
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
                this.NavigateBackCommand.OnCanExecuteChanged();
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
                this.NavigateBackCommand.OnCanExecuteChanged();
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
