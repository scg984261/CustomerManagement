using CustomerManagement.Command;
using CustomerManagement.Navigation;

namespace CustomerManagement.ViewModel
{
    public class NewServiceViewModel : ValidationViewModelBase
    {
        private NavigationStore navigationStore;
        private ServiceItemViewModel serviceItemViewModel;
        public static ServicesViewModel? ParentServicesViewModel { get; set; }
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
            if (ParentServicesViewModel != null)
            {
                ParentServicesViewModel.Services.Add(this.serviceItemViewModel);
            }

            this.NavigateBack();
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
