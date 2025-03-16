using CustomerManagement.Navigation;
using CustomerManagement.Command;

namespace CustomerManagement.ViewModel
{
    public class ServiceDetailsViewModel : ValidationViewModelBase
    {
        public static ServicesViewModel? ParentServicesViewModel;
        private static readonly string dateTimeFormat = "yyyy-MMM-dd HH:mm:ss";

        private NavigationStore navigationStore;
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        private ServiceItemViewModel serviceItemViewModel;
        
        private readonly string name;
        private readonly decimal price;

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
                    const string errorMessage = $"Value must be a valid decimal.";
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
            
            this.serviceItemViewModel = serviceItemViewModel;
            this.navigationStore = navigationStore;

            this.SaveCommand = new DelegateCommand(this.SaveService, this.CanSaveService);
            this.CancelCommand = new DelegateCommand(this.Cancel);
        }

        public void SaveService(object? parameter)
        {
            // Add a call to SQL Server via Entity Framework to save the service!

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

            this.NavigateBack();
        }
    }
}
