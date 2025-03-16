using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;
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
            NavigationStore navigationStore = new NavigationStore();
            CustomerDataProvider customerDataProvider = new CustomerDataProvider();
            CustomersViewModel customersViewModel = new CustomersViewModel(navigationStore, customerDataProvider);
            CustomerDetailsViewModel.ParentCustomersViewModel = customersViewModel;
            NewCustomerViewModel.ParentCustomersViewModel = customersViewModel;
            navigationStore.SelectedViewModel = customersViewModel;
            
            ServiceDataProvider serviceDataProvider = new ServiceDataProvider();
            ServicesViewModel servicesViewModel = new ServicesViewModel(navigationStore, serviceDataProvider);
            ServiceDetailsViewModel.ParentServicesViewModel = servicesViewModel;
            NewServiceViewModel.ParentServicesViewModel = servicesViewModel;
            MainViewModel mainViewModel = new MainViewModel(navigationStore, customersViewModel, servicesViewModel);
            MainWindow mainWindow = new MainWindow(mainViewModel);

            mainWindow?.Show();
        }
    }
}
