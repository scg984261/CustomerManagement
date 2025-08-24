using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.Windows;

namespace CustomerManagement.ViewModel.ServiceViewModels
{
    public class ServiceViewModelBase : ValidationViewModelBase
    {
        protected NavigationStore navigationStore;
        protected readonly IServiceDataProvider serviceDataProvider;
        protected IMessageBoxHelper messageBoxHelper;

        private string name;
        private decimal price;
        private bool isRecurring;

        protected string priceString;

        public DelegateCommand SaveServiceCommand { get; }
        public DelegateCommand NavigateBackCommand { get; }

        public static ServicesViewModel? ParentServicesViewModel;

        public ServiceViewModelBase(NavigationStore navigationStore, IServiceDataProvider serviceDataProvider, IMessageBoxHelper messageBoxHelper)
        {
            this.name = string.Empty;
            this.price = 0.0m;
            this.priceString = this.price.ToString();
            this.isRecurring = false;

            this.navigationStore = navigationStore;
            this.serviceDataProvider = serviceDataProvider;
            this.messageBoxHelper = messageBoxHelper;

            this.SaveServiceCommand = new DelegateCommand(this.SaveService, this.CanSaveService);
            this.NavigateBackCommand = new DelegateCommand(this.Cancel);
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;

                if (value != null)
                {
                    this.name = value;
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
                return this.price;
            }
            set
            {
                this.price = value;
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
                    this.Price = 0m;
                    this.NotifyPropertyChanged(nameof(PriceFormatted));
                    this.SaveServiceCommand.RaiseCanExecuteChanged();
                    return;
                }
                else
                {
                    this.ClearErrors();
                }

                decimal price;
                if (decimal.TryParse(value, out price))
                {
                    this.Price = price;
                    this.ClearErrors();
                }
                else
                {
                    const string errorMessage = "Value must be a valid decimal.";
                    this.Price = 0m;
                    this.AddError(errorMessage);
                }

                this.NotifyPropertyChanged(nameof(PriceFormatted));
                this.SaveServiceCommand.RaiseCanExecuteChanged();
            }
        }

        public string PriceFormatted
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
                return this.isRecurring;
            }
            set
            {
                this.isRecurring = value;
                this.NotifyPropertyChanged();
            }
        }

        public virtual void SaveService(object? parameter)
        {
            return;
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

        public virtual void Cancel(object? parameter)
        {
            this.NavigateBack(new object());
        }

        public void NavigateBack(object? parameter)
        {
            if (ParentServicesViewModel != null)
            {
                this.navigationStore.SelectedViewModel = ParentServicesViewModel;
                this.navigationStore.SelectedViewModel.Load();
            }
        }
    }
}
