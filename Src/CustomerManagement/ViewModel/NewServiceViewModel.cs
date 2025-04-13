using CDB.Model;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.Windows;
using log4net;

namespace CustomerManagement.ViewModel
{
    public class NewServiceViewModel : ValidationViewModelBase
    {
        private NavigationStore navigationStore;
        private readonly IServiceDataProvider serviceDataProvider;
        private IMessageBoxHelper messageBoxHelper;
        private ServiceItemViewModel serviceItemViewModel;
        private string priceString;

        private static readonly ILog log = LogManager.GetLogger(typeof(NewServiceViewModel));

        public DelegateCommand NavigateBackDelegateCommand { get; }
        public DelegateCommand SaveServiceCommand { get; }

        public static ServicesViewModel? ParentServicesViewModel { get; set; }

        public NewServiceViewModel(NavigationStore navigationStore, IServiceDataProvider serviceDataProvider, IMessageBoxHelper messageBoxHelper)
        {
            this.navigationStore = navigationStore;
            this.serviceDataProvider = serviceDataProvider;
            this.messageBoxHelper = messageBoxHelper;

            this.serviceItemViewModel = new ServiceItemViewModel();
            this.priceString = string.Empty;
            this.NavigateBackDelegateCommand = new DelegateCommand(this.NavigateBack);
            this.SaveServiceCommand = new DelegateCommand(this.SaveService, this.CanSaveService);
        }

        public string? Name
        {
            get
            {
                return this.serviceItemViewModel.Name;
            }
            set
            {
                if (value != null)
                {
                    this.serviceItemViewModel.Name = value;
                }

                if (string.IsNullOrEmpty(this.Name))
                {
                    const string errorMessage = "Name of service cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.NotifyPropertyChanged();
                this.SaveServiceCommand.RaiseCanExecuteChanged();
            }
        }

        public decimal Price
        {
            get
            {
                return this.serviceItemViewModel.Price;
            }
            set
            {
                this.serviceItemViewModel.Price = value;
            }
        }

        public string PriceString
        {
            get
            {
                return this.priceString;
            }
            set
            {
                this.priceString = value;

                if (string.IsNullOrEmpty(this.priceString))
                {
                    const string errorMessage = "Price cannot be blank!";
                    this.AddError(errorMessage);
                    this.serviceItemViewModel.Price = 0m;
                    this.NotifyPropertyChanged(nameof(PriceFormatted));
                    this.SaveServiceCommand.RaiseCanExecuteChanged();
                    return;
                }
                else
                {
                    this.ClearErrors();
                }

                decimal price;
                if (Decimal.TryParse(value, out price))
                {
                    this.serviceItemViewModel.Price = price;
                    this.ClearErrors();
                }
                else
                {
                    const string errorMessage = "Value must be a valid decimal.";
                    this.serviceItemViewModel.Price = 0m;
                    this.AddError(errorMessage);
                }

                this.NotifyPropertyChanged(nameof(PriceFormatted));
                this.SaveServiceCommand.RaiseCanExecuteChanged();
            }
        }

        public string  PriceFormatted
        {
            get
            {
                return $"£{this.Price.ToString("0.00")}";
            }
        }

        public bool IsRecurring
        { 
            get
            {
                return this.serviceItemViewModel.IsRecurring;
            }
            set
            {
                this.serviceItemViewModel.IsRecurring = value;
                this.NotifyPropertyChanged();
            }
        }

        public void NavigateBack(object? parameter)
        {
            if (ParentServicesViewModel != null)
            {
                this.navigationStore.SelectedViewModel = ParentServicesViewModel;
                this.navigationStore.SelectedViewModel.Load();
            }
        }

        public void SaveService(object? parameter)
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
        
        public bool CanSaveService(object? parameter)
        {
            if (this.HasErrors)
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.Name)) return false;
            if (string.IsNullOrEmpty(this.PriceString)) return false;

            return true;
        }
    }
}
