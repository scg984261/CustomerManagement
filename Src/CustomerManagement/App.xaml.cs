using CDB;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;
using CustomerManagement.ViewModel.CustomerViewModels;
using CustomerManagement.ViewModel.ServiceViewModels;
using CustomerManagement.Windows;
using System.Windows;

namespace CustomerManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DataWrapper dataWrapper = new DataWrapper();

            NavigationStore navigationStore = new NavigationStore();
            CustomerDataProvider customerDataProvider = new CustomerDataProvider(dataWrapper);
            CustomersViewModel customersViewModel = new CustomersViewModel(navigationStore, customerDataProvider);
            CustomerDetailsViewModel.ParentCustomersViewModel = customersViewModel;
            NewCustomerViewModel.ParentCustomersViewModel = customersViewModel;
            
            ServiceDataProvider serviceDataProvider = new ServiceDataProvider(dataWrapper);
            ServicesViewModel servicesViewModel = new ServicesViewModel(navigationStore, serviceDataProvider, new MessageBoxHelper());
            ServiceDetailsViewModel.ParentServicesViewModel = servicesViewModel;
            NewServiceViewModel.ParentServicesViewModel = servicesViewModel;
            MainViewModel mainViewModel = new MainViewModel(navigationStore, customersViewModel, servicesViewModel);
            MainWindow mainWindow = new MainWindow(mainViewModel);

            navigationStore.SelectedViewModel = customersViewModel;

            mainWindow?.Show();
        }
    }
}
