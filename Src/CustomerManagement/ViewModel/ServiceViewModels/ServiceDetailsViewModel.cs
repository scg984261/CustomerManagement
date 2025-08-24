using CustomerManagement.Navigation;
using CustomerManagement.Data;
using CustomerManagement.Windows;
using log4net;

namespace CustomerManagement.ViewModel.ServiceViewModels
{
    public class ServiceDetailsViewModel : ServiceViewModelBase
    {
        private ServiceItemViewModel serviceItemViewModel;

        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceDetailsViewModel));
        private static readonly string dateTimeFormat = "dd-MMM-yyyy HH:mm:ss";

        private readonly string initialName;
        private readonly decimal initialPrice;
        private readonly bool initialIsRecurring;

        public ServiceDetailsViewModel(ServiceItemViewModel serviceItemViewModel, IServiceDataProvider serviceDataProvider, NavigationStore navigationStore, IMessageBoxHelper messageBoxHelper) : base(navigationStore, serviceDataProvider, messageBoxHelper)
        {
            this.Name = serviceItemViewModel.Name;
            this.Price = serviceItemViewModel.Price;
            this.PriceString = this.Price.ToString();
            this.IsRecurring = serviceItemViewModel.IsRecurring;

            this.initialName = this.Name;
            this.initialPrice = this.Price;
            this.initialIsRecurring = this.IsRecurring;

            this.serviceItemViewModel = serviceItemViewModel;
            this.navigationStore = navigationStore;
        }

        public int Id
        {
            get
            {
                return this.serviceItemViewModel.Id;
            }
        }

        public string CreatedDateTime
        {
            get
            {
                return this.serviceItemViewModel.CreatedDateTime.ToString(dateTimeFormat);
            }
        }

        public string LastUpdateDateTime
        {
            get
            {
                return this.serviceItemViewModel.LastUpdateDateTime.ToString(dateTimeFormat);
            }
        }

        public override void SaveService(object? parameter)
        {
            try
            {
                this.serviceItemViewModel.Name = this.Name;
                this.serviceItemViewModel.Price = this.Price;
                this.serviceItemViewModel.IsRecurring = this.IsRecurring;

                int inputServiceResult = this.serviceDataProvider.UpdateService(this.Id);
                this.messageBoxHelper.ShowInfoDialog($"Service record with ID {this.Id} updated.", "Service Updated");
                this.NavigateBack(new object());
            }
            catch (Exception exception)
            {
                log.Error($"Error occurred attempting to update service with ID {this.Id}.");
                log.Error(exception);
                this.messageBoxHelper.ShowErrorDialog(exception, "Error updating service");
                this.Cancel(parameter);
            }
        }

        public override void Cancel(object? parameter)
        {
            // Reset Service fields to their original values prior to navigating back.
            this.serviceItemViewModel.Name = this.initialName;
            this.serviceItemViewModel.Price = this.initialPrice;
            this.serviceItemViewModel.IsRecurring = this.initialIsRecurring;

            this.Name = this.initialName;
            this.Price = this.initialPrice;
            this.IsRecurring = this.initialIsRecurring;
            base.Cancel(parameter);
        }
    }
}
