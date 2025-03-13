using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;
using System.Configuration;
using System.Data;
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
            navigationStore.SelectedViewModel = customersViewModel;
            ServiceDataProvider serviceDataProvider = new ServiceDataProvider();
            ServicesViewModel servicesViewModel = new ServicesViewModel(serviceDataProvider);
            MainViewModel mainViewModel = new MainViewModel(navigationStore, customersViewModel, servicesViewModel);
            MainWindow mainWindow = new MainWindow(mainViewModel);
            
            mainWindow?.Show();
        }
    }
}
;