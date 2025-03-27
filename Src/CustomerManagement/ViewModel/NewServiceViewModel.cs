using CDB.Model;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using log4net;
using System.Windows;

namespace CustomerManagement.ViewModel
{
    public class NewServiceViewModel : ValidationViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NewServiceViewModel));

        private NavigationStore navigationStore;
        private ServiceItemViewModel serviceItemViewModel;
        public static ServicesViewModel? ParentServicesViewModel { get; set; }
        private static readonly ServiceDataProvider serviceDataProvider = new ServiceDataProvider();
        public DelegateCommand NavigateBackDelegateCommand { get; }
        public DelegateCommand SaveServiceCommand { get; }
        private string priceString;

        public NewServiceViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
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
                    const string errorMessage = $"Value must be a valid decimal.";
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
            this.NavigateBack();
        }

        public void NavigateBack()
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
                serviceDataProvider.InsertNewService(service);
                ServiceItemViewModel serviceItemViewModel = new ServiceItemViewModel(service);

                if (ParentServicesViewModel != null)
                {
                    ParentServicesViewModel.Services.Add(serviceItemViewModel);
                }
                
                log.Debug($"New Service with ID {serviceItemViewModel.Id} successfully added.");
                MessageBox.Show($"New Service inserted into the database with ID {serviceItemViewModel.Id}.", "New Service Added", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                log.Error(exception);
                string errorMessage = $"Exception {exception.GetType().FullName} occurred attempting to insert new service record into the database.\r\n";
                errorMessage += "Service was not inserted. Please see the logs for more information.";
                MessageBox.Show(errorMessage, "Error Inserting Service", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.NavigateBack();
            }
        }
        
        public bool CanSaveService(object? parameter)
        {
            if (string.IsNullOrEmpty(this.Name)) return false;
            if (string.IsNullOrEmpty(this.PriceString)) return false;

            if (this.HasErrors) return false;

            return true;
        }
    }
}
