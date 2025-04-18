using System.Collections.ObjectModel;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.Windows;
using CDB.Model;
using log4net;

namespace CustomerManagement.ViewModel.ServiceViewModels
{
    public class ServicesViewModel : ViewModelBase
    {
        private NavigationStore navigationStore;
        private ServiceItemViewModel? selectedService;
        private readonly IMessageBoxHelper messageBoxHelper;
        private readonly IServiceDataProvider serviceDataProvider;

        private static readonly ILog log = LogManager.GetLogger(typeof(ServicesViewModel));

        public ObservableCollection<ServiceItemViewModel> Services { get; } = new ObservableCollection<ServiceItemViewModel>();

        public DelegateCommand ServiceDetailsCommand { get; }
        public DelegateCommand NavigateNewServiceCommand { get; }

        public ServicesViewModel(NavigationStore navigationStore, IServiceDataProvider serviceDataProvider, IMessageBoxHelper messageBoxHelper)
        {
            this.navigationStore = navigationStore;
            this.serviceDataProvider = serviceDataProvider;
            this.messageBoxHelper = messageBoxHelper;
            this.ServiceDetailsCommand = new DelegateCommand(this.NavigateToDetails, this.IsServiceSelected);
            this.NavigateNewServiceCommand = new DelegateCommand(this.NavigateToNewService);
        }

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

        public override void Load()
        {
            try
            {
                if (this.Services.Any())
                {
                    return;
                }
           
                List<Service> services = this.serviceDataProvider.GetAll();

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
                log.Error(exception);
                this.messageBoxHelper.ShowErrorDialog(exception, "Error Loading Services");
                throw;
            }
        }

        public void NavigateToDetails(object? parameter)
        {
            if (this.SelectedService != null)
            {
                ServiceDetailsViewModel serviceDetailsViewModel = new ServiceDetailsViewModel(this.SelectedService, this.serviceDataProvider, this.navigationStore, new MessageBoxHelper());
                this.navigationStore.SelectedViewModel = serviceDetailsViewModel;
            }
        }

        public bool IsServiceSelected(object? parameter)
        {
            return this.SelectedService != null;
        }

        public void NavigateToNewService(object? parameter)
        {
            this.navigationStore.SelectedViewModel = new NewServiceViewModel(this.navigationStore, this.serviceDataProvider, new MessageBoxHelper());
        }
    }
}
