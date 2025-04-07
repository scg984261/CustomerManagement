using System.Windows;
using System.Collections.ObjectModel;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CDB.Model;

namespace CustomerManagement.ViewModel
{
    public class ServicesViewModel : ViewModelBase
    {
        public ObservableCollection<ServiceItemViewModel> Services { get; } = new ObservableCollection<ServiceItemViewModel>();
        private NavigationStore navigationStore;

        private ServiceItemViewModel? selectedService;

        public ServiceItemViewModel? SelectedService
        {
            get
            {
                return this.selectedService;
            }
            set
            {
                this.selectedService = value;
                this.NotifyPropertyChanged();
                this.ServiceDetailsCommand.RaiseCanExecuteChanged();
            }
        }
        private readonly IServiceDataProvider serviceDataProvider;
        public DelegateCommand ServiceDetailsCommand { get; }
        public DelegateCommand NavigateNewServiceCommand { get; }

        public ServicesViewModel(NavigationStore navigationStore, IServiceDataProvider serviceDataProvider)
        {
            this.navigationStore = navigationStore;
            this.serviceDataProvider = new ServiceDataProvider();
            this.ServiceDetailsCommand = new DelegateCommand(this.NavigateToDetails, this.IsServiceSelected);
            this.NavigateNewServiceCommand = new DelegateCommand(this.NavigateToNewService);
        }

        public override void Load()
        {
            try
            {
                if (this.Services.Any())
                {
                    return;
                }
           
                var services = this.serviceDataProvider.GetAll();

                if (services != null)
                {
                    foreach (Service service in services)
                    {
                        ServiceItemViewModel serviceItemViewModel = new ServiceItemViewModel(service);
                        this.Services.Add(serviceItemViewModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string errorMessage = $"{exception.GetType().FullName} ({exception.HResult}) - {exception.Message}";
                MessageBox.Show($"{errorMessage}", "Error Updating Customer", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public void NavigateToDetails(object? parameter)
        {
            if (this.SelectedService != null)
            {
                ServiceDetailsViewModel serviceDetailsViewModel = new ServiceDetailsViewModel(this.SelectedService, this.navigationStore);
                this.navigationStore.SelectedViewModel = serviceDetailsViewModel;
            }
        }

        public bool IsServiceSelected(object? parameter)
        {
            return this.SelectedService != null;
        }

        public void NavigateToNewService(object? parameter)
        {
            this.navigationStore.SelectedViewModel = new NewServiceViewModel(this.navigationStore);
        }
    }
}
