using System.Windows;
using CustomerManagement.Navigation;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CDB.Model;
using log4net;

namespace CustomerManagement.ViewModel
{
    public class NewCustomerViewModel : ValidationViewModelBase
    {
        public static CustomersViewModel? ParentCustomersViewModel { get; set; }
        private static readonly CustomerDataProvider customerDataProvider = new CustomerDataProvider();
        private static readonly ILog log = LogManager.GetLogger(typeof(NewCustomerViewModel));
        private NavigationStore navigationStore;

        public DelegateCommand NavigateBackCommand { get; }
        public DelegateCommand SaveCustomerCommand { get; }

        private string? companyName;
        private string? businessContact;
        private string? emailAddress;
        private string? contactNumber;

        public NewCustomerViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.NavigateBackCommand = new DelegateCommand(this.NavigateBack);
            this.SaveCustomerCommand = new DelegateCommand(this.SaveCustomer, this.CanSaveCustomer);
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

                if (string.IsNullOrWhiteSpace(this.companyName))
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

                if (string.IsNullOrWhiteSpace(this.businessContact))
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

                if (string.IsNullOrWhiteSpace(this.contactNumber))
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

                if (string.IsNullOrWhiteSpace(this.emailAddress))
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
            try
            {
                Customer customer = new Customer(this.companyName, this.businessContact, this.emailAddress, this.contactNumber);
                customerDataProvider.InsertNewCustomer(customer);
                CustomerItemViewModel customerItemViewModel = new CustomerItemViewModel(customer);

                if (ParentCustomersViewModel != null)
                {
                    ParentCustomersViewModel.Customers.Add(customerItemViewModel);
                }

                log.Info($"Customer with ID {customerItemViewModel.Id} successfully added.");
            }
            catch(Exception exception)
            {
                log.Error(exception);
                string errorMessage = $"Exception {exception.GetType().FullName} occurred attempting to insert new customer into the database.\r\n";
                errorMessage += "Customer was not inserted. Please see the logs for more information.";
                MessageBox.Show(errorMessage, "Error Inserting Customer", MessageBoxButton.OK, MessageBoxImage.Error);
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
