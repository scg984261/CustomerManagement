using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.Windows;
using CDB.Model;
using log4net;

namespace CustomerManagement.ViewModel.ServiceViewModels
{
    public class NewServiceViewModel : ServiceViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NewServiceViewModel));

        public NewServiceViewModel(NavigationStore navigationStore, IServiceDataProvider serviceDataProvider, IMessageBoxHelper messageBoxHelper) : base(navigationStore, serviceDataProvider, messageBoxHelper)
        {
        }

        public override void SaveService(object? parameter)
        {
            try
            {
                Service service = new Service(this.Name, this.Price, this.IsRecurring);
                int insertServiceResult = serviceDataProvider.InsertNewService(service);
                ServiceItemViewModel serviceItemViewModel = new ServiceItemViewModel(service);

                if (ParentServicesViewModel != null)
                {
                    ParentServicesViewModel.Services.Add(serviceItemViewModel);
                }
                
                log.Debug($"New Service with ID {serviceItemViewModel.Id} successfully added.");
                this.messageBoxHelper.ShowInfoDialog($"New Service inserted into the database with ID {serviceItemViewModel.Id}.", "New Service Added");
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception {exception.GetType().FullName} occurred attempting to insert new service record into the database.\r\n";
                errorMessage += "Service was not inserted. Please see the logs for more information.";
                log.Error(errorMessage);
                log.Error(exception);
                this.messageBoxHelper.ShowErrorDialog(exception, "Error Inserting Service");
            }
            finally
            {
                this.NavigateBack(new object());
            }
        }
    }
}
