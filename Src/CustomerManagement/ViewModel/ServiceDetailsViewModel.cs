using CustomerManagement.Navigation;
using CustomerManagement.Command;

namespace CustomerManagement.ViewModel
{
    public class ServiceDetailsViewModel : ValidationViewModelBase
    {
        private NavigationStore navigationStore;
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        private ServiceItemViewModel serviceItemViewModel;
        public static ServicesViewModel? ParentServicesViewModel;

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

        public ServiceDetailsViewModel(ServiceItemViewModel serviceItemViewModel, NavigationStore navigationStore)
        {
            this.name = serviceItemViewModel.Name;
            this.price = serviceItemViewModel.Price;
            
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

        public async void NavigateBack()
        {
            if (ParentServicesViewModel != null)
            {
                this.navigationStore.SelectedViewModel = ParentServicesViewModel;
                await this.navigationStore.SelectedViewModel.LoadAsync();
            }
        }

        public bool CanSaveService(object? parameter)
        {
            return false;
        }

        public void Cancel(object? parameter)
        {
            this.serviceItemViewModel.Name = this.name;
            this.serviceItemViewModel.Price = this.price;

            this.NavigateBack();
        }
    }
}
