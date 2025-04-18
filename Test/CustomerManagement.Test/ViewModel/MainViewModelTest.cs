using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;
using CustomerManagement.ViewModel.CustomerViewModels;
using CustomerManagement.ViewModel.ServiceViewModels;
using CustomerManagement.Data;
using CDB.Model;
using Moq;
using CustomerManagement.Windows;
using Microsoft.Identity.Client;

namespace CustomerManagement.Test.ViewModel
{
    public class MainViewModelTest
    {
        private NavigationStore testNavigationStore;

        private Mock<ICustomerDataProvider> mockCustomerDataProvider;
        private ICustomerDataProvider mockCustomerDataProviderObject;

        private CustomersViewModel testCustomersViewModel;

        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper mockMessageBoxHelperObject;

        private Mock<IServiceDataProvider> mockServiceDataProvider;
        private IServiceDataProvider mockServiceDataProviderObject;

        private ServicesViewModel testServicesViewModel;

        private MainViewModel testMainViewModel;

        [SetUp]
        public void Setup()
        {
            this.testNavigationStore = new NavigationStore();

            this.mockCustomerDataProvider = new Mock<ICustomerDataProvider>();
            this.mockCustomerDataProviderObject = this.mockCustomerDataProvider.Object;

            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.mockMessageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.mockServiceDataProvider = new Mock<IServiceDataProvider>();
            this.mockServiceDataProviderObject = this.mockServiceDataProvider.Object;

            this.testCustomersViewModel = new CustomersViewModel(this.testNavigationStore, this.mockCustomerDataProviderObject, this.mockMessageBoxHelperObject);
            this.testServicesViewModel = new ServicesViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.mockMessageBoxHelperObject);

            this.testMainViewModel = new MainViewModel(this.testNavigationStore, this.testCustomersViewModel, this.testServicesViewModel);
        }

        [Test]
        public void TestConstructor()
        {
            this.testMainViewModel = new MainViewModel(this.testNavigationStore, this.testCustomersViewModel, this.testServicesViewModel);

            Assert.That(this.testMainViewModel.CustomersViewModel, Is.Not.Null);
            Assert.That(this.testMainViewModel.ServicesViewModel, Is.Not.Null);
            Assert.That(this.testMainViewModel.SelectViewModelCommand, Is.Not.Null);
            Assert.That(this.testMainViewModel.SelectedViewModel, Is.Null);
        }

        [Test]
        public void TestSelectedViewModel()
        {
            // Arrange/Act.
            // Select a view model - CustomersViewModel.
            this.testMainViewModel.SelectedViewModel = this.testCustomersViewModel;

            // Assert.
            Assert.That(this.testMainViewModel.SelectedViewModel, Is.Not.Null);
            Assert.That(this.testMainViewModel.SelectedViewModel is CustomersViewModel);
            Assert.That(this.testMainViewModel.SelectedViewModel.GetType().FullName, Is.EqualTo("CustomerManagement.ViewModel.CustomerViewModels.CustomersViewModel"));
        }

        [Test]
        public void TestLoad()
        {
            // Arrange.
            // Set the SelectedViewModel to the CustomersViewModel.
            this.testMainViewModel.SelectedViewModel = this.testCustomersViewModel;

            // Mock the Customers to be returned by the database.
            List<Customer> testCustomerList = new List<Customer>
            {
                new Customer(1, "Test company name 1", "test business contact 1", "testemailaddress@hotmail.com", "01134546987", new DateTime(2025, 04, 16, 17, 19, 14), new DateTime(2025, 04, 16, 17, 19, 16)),
                new Customer(2, "Test company name 2", "test business contact 2", "testemailaddress@outlook.com", "9847666515", new DateTime(2025, 04, 16, 17, 19, 15), new DateTime(2025, 04, 16, 17, 19, 17)),
                new Customer(3, "Test company name 3", "test business contact 3", "testemailaddress@gmail.com", "548844898", new DateTime(2025, 04, 16, 17, 19, 15), new DateTime(2025, 04, 16, 17, 19, 18)),
                new Customer(4, "Test company name 4", "test business contact 4", "testemailaddress@yahoo.co.uk", "44489811036997", new DateTime(2025, 04, 16, 17, 19, 15), new DateTime(2025, 04, 16, 17, 19, 19)),
            };

            this.mockCustomerDataProvider.Setup(dataProvider => dataProvider.GetAll()).Returns(testCustomerList);

            // Act.
            this.testMainViewModel.Load();

            // Assert.
            Assert.That(this.testMainViewModel.SelectedViewModel, Is.Not.Null);
            Assert.That(this.testMainViewModel.SelectedViewModel is CustomersViewModel);
            Assert.That(this.testMainViewModel.SelectedViewModel.GetType().FullName, Is.EqualTo("CustomerManagement.ViewModel.CustomerViewModels.CustomersViewModel"));

            CustomersViewModel? selectedViewModel = this.testMainViewModel.SelectedViewModel as CustomersViewModel;
            Assert.That(selectedViewModel, Is.Not.Null);
            Assert.That(selectedViewModel.Customers.Count, Is.EqualTo(4));
        }

        [Test]
        public void TestSelectViewModel_ShouldSelectViewModel()
        {
            // Arrange.
            // Set the SelectedViewModel to the CustomersViewModel.
            this.testMainViewModel.SelectedViewModel = this.testCustomersViewModel;

            // Mock the Services to be returned by the service data provider.
            List<Service> testServiceList = new List<Service>
            {
                new Service(1, "Test service 1", 0.99m, true, new DateTime(2025, 04, 16, 19, 15, 49), new DateTime(2025, 04, 16, 19, 15, 49)),
                new Service(2, "Test service 2", 1.89m, true, new DateTime(2025, 04, 16, 19, 15, 50), new DateTime(2025, 04, 16, 19, 15, 50)),
                new Service(3, "Test service 3", 0.79m, true, new DateTime(2025, 04, 16, 19, 15, 52), new DateTime(2025, 04, 16, 19, 15, 53)),
            };

            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.GetAll()).Returns(testServiceList);

            // Act.
            this.testMainViewModel.SelectViewModel(this.testServicesViewModel);

            // Assert.
            Assert.That(this.testMainViewModel.SelectedViewModel, Is.Not.Null);
            Assert.That(this.testMainViewModel.SelectedViewModel is ServicesViewModel);
            Assert.That(this.testMainViewModel.SelectedViewModel.GetType().FullName, Is.EqualTo("CustomerManagement.ViewModel.ServiceViewModels.ServicesViewModel"));

            ServicesViewModel? selectedViewModel = this.testMainViewModel.SelectedViewModel as ServicesViewModel;
            Assert.That(selectedViewModel, Is.Not.Null);
            Assert.That(selectedViewModel.Services.Count, Is.EqualTo(3));
        }
    }
}
