using CustomerManagement.Navigation;
using CustomerManagement.Command;

namespace CustomerManagement.ViewModel
{
    public class NewCustomerViewModel : ValidationViewModelBase
    {
        public static CustomersViewModel? ParentCustomersViewModel { get; set; }
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;

        public DelegateCommand NavigateBackCommand { get; }
        public DelegateCommand SaveCustomerCommand { get; }

        public NewCustomerViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.NavigateBackCommand = new DelegateCommand(this.NavigateBack);
            this.SaveCustomerCommand = new DelegateCommand(this.SaveCustomer, this.CanSaveCustomer);
            this.customerItemViewModel = new CustomerItemViewModel();
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
                this.NotifyPropertyChanged();

                if (string.IsNullOrWhiteSpace(this.customerItemViewModel.CompanyName))
                {
                    const string errorMessage = "Company name cannot be blank.";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.SaveCustomerCommand.RaiseCanExecuteChanged();
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
                this.NotifyPropertyChanged();

                if (string.IsNullOrWhiteSpace(this.customerItemViewModel.BusinessContact))
                {
                    const string errorMessage = "Business contact cannot be blank.";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.SaveCustomerCommand.RaiseCanExecuteChanged();
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

                if (string.IsNullOrWhiteSpace(this.customerItemViewModel.ContactNumber))
                {
                    const string errorMessage = "Contact Number cannot be blank.";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.SaveCustomerCommand.RaiseCanExecuteChanged();
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
                this.NotifyPropertyChanged();

                if (string.IsNullOrWhiteSpace(this.customerItemViewModel.EmailAddress))
                {
                    const string errorMessage = "Email Address cannot be blank.";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.SaveCustomerCommand.RaiseCanExecuteChanged();
            }
        }

        public void NavigateBack(object? parameter)
        {
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
            // Add call to CDB here via Entity Framework!
            if (ParentCustomersViewModel != null)
            {
                ParentCustomersViewModel.Customers.Add(this.customerItemViewModel);
            }
            
            this.NavigateBack();
        }

        public bool CanSaveCustomer(object? parameter)
        {
            if (string.IsNullOrEmpty(this.CompanyName)) return false;
            if (string.IsNullOrEmpty(this.BusinessContact)) return false;
            if (string.IsNullOrEmpty(this.ContactNumber)) return false;
            if (string.IsNullOrEmpty(this.EmailAddress)) return false;

            if (this.HasErrors)
            {
                return false;
            }

            return true;
        }
    }
}
