using System.Windows;
using CustomerManagement.Navigation;
using CustomerManagement.Command;
using CustomerManagement.Data;
using log4net;

namespace CustomerManagement.ViewModel
{
    public class ServiceDetailsViewModel : ValidationViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceDetailsViewModel));
        public static ServicesViewModel? ParentServicesViewModel;
        private static readonly string dateTimeFormat = "dd-MMM-yyyy HH:mm:ss";
        private static readonly ServiceDataProvider serviceDataProvider = new ServiceDataProvider();
        private NavigationStore navigationStore;
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        private ServiceItemViewModel serviceItemViewModel;

        
        private readonly string name;
        private readonly decimal price;
        private readonly bool isRecurring;

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

        private string priceString;

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

        public ServiceDetailsViewModel(ServiceItemViewModel serviceItemViewModel, NavigationStore navigationStore)
        {
            this.name = serviceItemViewModel.Name;
            this.price = serviceItemViewModel.Price;
            this.priceString = serviceItemViewModel.Price.ToString();
            this.isRecurring = serviceItemViewModel.IsRecurring;
            
            this.serviceItemViewModel = serviceItemViewModel;
            this.navigationStore = navigationStore;

            this.SaveCommand = new DelegateCommand(this.SaveService, this.CanSaveService);
            this.CancelCommand = new DelegateCommand(this.Cancel);
        }

        public void SaveService(object? parameter)
        {
            try
            {
                serviceDataProvider.UpdateService(this.Id);
                MessageBox.Show($"Service record with ID {this.Id} updated.", "Service Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigateBack();
            }
            catch (Exception exception)
            {
                log.Error(exception);
                MessageBox.Show($"Error occurred attempting to update service.\r\nService was not updated.\r\nPlease see the log file for more information.", "Error Updating Service", MessageBoxButton.OK, MessageBoxImage.Error);
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
