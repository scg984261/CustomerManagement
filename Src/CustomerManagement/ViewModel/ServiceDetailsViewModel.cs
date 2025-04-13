using CustomerManagement.Navigation;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Windows;
using log4net;

namespace CustomerManagement.ViewModel
{
    public class ServiceDetailsViewModel : ValidationViewModelBase
    {
        private ServiceItemViewModel serviceItemViewModel;
        private NavigationStore navigationStore;
        private IMessageBoxHelper messageBoxHelper;

        private readonly string name;
        private readonly decimal price;
        private readonly bool isRecurring;

        private string priceString;
        private readonly IServiceDataProvider serviceDataProvider;

        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceDetailsViewModel));
        private static readonly string dateTimeFormat = "dd-MMM-yyyy HH:mm:ss";

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public static ServicesViewModel? ParentServicesViewModel;

        public ServiceDetailsViewModel(ServiceItemViewModel serviceItemViewModel, IServiceDataProvider serviceDataProvider, NavigationStore navigationStore, IMessageBoxHelper messageBoxHelper)
        {
            this.name = serviceItemViewModel.Name;
            this.price = serviceItemViewModel.Price;
            this.isRecurring = serviceItemViewModel.IsRecurring;

            this.serviceDataProvider = serviceDataProvider;

            this.serviceItemViewModel = serviceItemViewModel;
            this.navigationStore = navigationStore;

            this.messageBoxHelper = messageBoxHelper;

            this.priceString = this.Price.ToString();

            this.SaveCommand = new DelegateCommand(this.SaveService, this.CanSaveService);
            this.CancelCommand = new DelegateCommand(this.Cancel);
        }

        public int Id
        {
            get
            {
                return this.serviceItemViewModel.Id;
            }
        }

        public string Name
        {
            get
            {
                return this.serviceItemViewModel.Name;
            }
            set
            {
                this.serviceItemViewModel.Name = value;

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
                this.SaveCommand.RaiseCanExecuteChanged();
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
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string PriceFormatted
        {
            get
            {
                return this.serviceItemViewModel.PriceFormatted;
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

        public void SaveService(object? parameter)
        {
            try
            {
                int inputServiceResult = serviceDataProvider.UpdateService(this.Id);
                this.messageBoxHelper.ShowInfoDialog($"Service record with ID {this.Id} updated.", "Service Updated");
                this.NavigateBack();
            }
            catch (Exception exception)
            {
                log.Error($"Error occurred attempting to update service with ID {this.Id}.");
                log.Error(exception);
                this.messageBoxHelper.ShowErrorDialog(exception, "Error updating service");
                this.Cancel(parameter);
            }
        }

        public void NavigateBack()
        {
            if (ParentServicesViewModel != null)
            {
                this.navigationStore.SelectedViewModel = ParentServicesViewModel;
                this.navigationStore.SelectedViewModel.Load();
            }
        }

        public bool CanSaveService(object? parameter)
        {
            if (string.IsNullOrEmpty(this.Name)) return false;
            if (string.IsNullOrEmpty(this.PriceString)) return false;

            if (this.HasErrors) return false;

            return true;
        }

        public void Cancel(object? parameter)
        {
            this.serviceItemViewModel.Name = this.name;
            this.serviceItemViewModel.Price = this.price;
            this.serviceItemViewModel.IsRecurring = this.isRecurring;
            this.NavigateBack();
        }
    }
}
