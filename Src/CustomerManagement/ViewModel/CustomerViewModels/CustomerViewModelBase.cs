using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.Windows;
using CustomerManagement.Command;

namespace CustomerManagement.ViewModel.CustomerViewModels
{
    public class CustomerViewModelBase : ValidationViewModelBase
    {
        private string? companyName;
        private string? businessContact;
        private string? emailAddress;
        private string? contactNumber;
        private bool isActive;

        protected NavigationStore navigationStore;
        protected ICustomerDataProvider customerDataProvider;
        protected IMessageBoxHelper messageBoxHelper;

        public DelegateCommand SaveCustomerCommand { get; set; }
        public DelegateCommand NavigateBackCommand { get; set; }

        public static CustomersViewModel? ParentCustomersViewModel { get; set; }

        public CustomerViewModelBase(NavigationStore navigationStore, ICustomerDataProvider customerDataProvider, IMessageBoxHelper messageBoxHelper)
        {
            this.companyName = string.Empty;
            this.businessContact = string.Empty;
            this.emailAddress = string.Empty;
            this.contactNumber = string.Empty;
            this.isActive = false;

            this.navigationStore = navigationStore;
            this.customerDataProvider = customerDataProvider;
            this.messageBoxHelper = messageBoxHelper;

            this.SaveCustomerCommand = new DelegateCommand(this.SaveCustomer, this.CanSaveCustomer);
            this.NavigateBackCommand = new DelegateCommand(this.NavigateBack);
        }

        public string? CompanyName
        {
            get
            {
                return this.companyName;
            }
            set
            {
                this.companyName = value;
                this.NotifyPropertyChanged();

                if (string.IsNullOrWhiteSpace(this.CompanyName))
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
                return this.businessContact;
            }

            set
            {
                this.businessContact = value;
                this.NotifyPropertyChanged();

                if (string.IsNullOrWhiteSpace(this.BusinessContact))
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
                return this.contactNumber;
            }
            set
            {
                this.contactNumber = value;
                this.NotifyPropertyChanged();

                if (string.IsNullOrWhiteSpace(this.ContactNumber))
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
                return this.emailAddress;
            }

            set
            {
                this.emailAddress = value;
                this.NotifyPropertyChanged();

                if (string.IsNullOrWhiteSpace(this.EmailAddress))
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

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
                this.NotifyPropertyChanged();
            }
        }

        public virtual void SaveCustomer(object? parameter)
        {
        }

        public bool CanSaveCustomer(object? parameter)
        {
            if (this.HasErrors)
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.CompanyName)) return false;
            if (string.IsNullOrEmpty(this.BusinessContact)) return false;
            if (string.IsNullOrEmpty(this.ContactNumber)) return false;
            if (string.IsNullOrEmpty(this.EmailAddress)) return false;

            return true;
        }

        public void NavigateBack(object? parameter)
        {
            if (ParentCustomersViewModel != null)
            {
                this.navigationStore.SelectedViewModel = ParentCustomersViewModel;
                this.navigationStore.SelectedViewModel.Load();
            }
        }
    }
}
