using CustomerManagement.Navigation;
using CustomerManagement.Data;
using CustomerManagement.Windows;
using CDB.Model;
using log4net;


namespace CustomerManagement.ViewModel.CustomerViewModels
{
    public class NewCustomerViewModel : CustomerViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NewCustomerViewModel));

        public NewCustomerViewModel(NavigationStore navigationStore, ICustomerDataProvider customerDataProvider, IMessageBoxHelper messageBoxHelper) : base(navigationStore, customerDataProvider, messageBoxHelper)
        {
        }

        public override void SaveCustomer(object? parameter)
        {
            
            try
            {
                Customer customer = new Customer(this.CompanyName, this.BusinessContact, this.EmailAddress, this.ContactNumber);
                int result = customerDataProvider.InsertNewCustomer(customer);
                CustomerItemViewModel customerItemViewModel = new CustomerItemViewModel(customer);

                if (ParentCustomersViewModel != null)
                {
                    ParentCustomersViewModel.Customers.Add(customerItemViewModel);
                }

                string message = $"Customer with ID {customerItemViewModel.Id} successfully added.";
                this.messageBoxHelper.ShowInfoDialog(message, "New Customer Added");
                log.Info(message);
            }
            catch (Exception exception)
            {
                log.Error(exception);
                string errorMessage = $"Exception {exception.GetType().FullName} occurred attempting to insert new customer into the database.\r\n";
                errorMessage += "Customer was not inserted. Please see the logs for more information.";
                this.messageBoxHelper.ShowErrorDialog(errorMessage, "Error Inserting Customer");
            }
            finally
            {
                this.NavigateBack(new object());
            }
        }
    }
}
