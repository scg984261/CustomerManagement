using System.Data;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.Windows;
using CustomerManagement.ViewModel.ServiceViewModels;
using CDB.Model;
using Moq;

namespace CustomerManagement.Test.ViewModel.ServiceViewModels
{
    public class NewServiceViewModelTest
    {
        private NavigationStore testNavigationStore;
        private Mock<IServiceDataProvider> mockServiceDataProvider;
        private IServiceDataProvider mockServiceDataProviderObject;
        private NewServiceViewModel testNewServiceViewModel;
        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper messageBoxHelperObject;
        private ServicesViewModel testServicesViewModel;

        [SetUp]
        public void Setup()
        {
            this.testNavigationStore = new NavigationStore();
            this.mockServiceDataProvider = new Mock<IServiceDataProvider>();
            this.mockServiceDataProviderObject = this.mockServiceDataProvider.Object;
            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.messageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testServicesViewModel = new ServicesViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);
            ServiceViewModelBase.ParentServicesViewModel = this.testServicesViewModel;

            this.testNewServiceViewModel = new NewServiceViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            this.testNewServiceViewModel = new NewServiceViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);

            Assert.That(this.testNewServiceViewModel.SaveServiceCommand, Is.Not.Null);
            Assert.That(this.testNewServiceViewModel.Name, Is.EqualTo(string.Empty));
            Assert.That(this.testNewServiceViewModel.Price, Is.EqualTo(0.0m));
            Assert.That(this.testNewServiceViewModel.PriceString, Is.EqualTo("0.0"));
            Assert.That(this.testNewServiceViewModel.IsRecurring, Is.False);
        }

        [Test]
        public void TestSaveService_ShouldSuccessfullySaveService()
        {
            // Arrange.
            this.testNewServiceViewModel.Name = "Test service name";
            this.testNewServiceViewModel.PriceString = "1.25";
            this.testNewServiceViewModel.IsRecurring = true;
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.True);

            // Set up mock data provider.
            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.InsertNewService(It.IsAny<Service>())).Returns(1);

            // Act.
            // Save the service.
            this.testNewServiceViewModel.SaveService(new object());

            // Assert.
            // Customer should have been successfully added.
            Assert.That(ServiceViewModelBase.ParentServicesViewModel, Is.Not.Null);
            Assert.That(ServiceViewModelBase.ParentServicesViewModel.Services.Count, Is.EqualTo(1));
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }

        [Test]
        public void TestSaveService_ShouldCatchException()
        {
            // Arrange.
            this.testNewServiceViewModel.Name = "Test invalid Service name";
            this.testNewServiceViewModel.PriceString = "5.84";
            this.testNewServiceViewModel.IsRecurring = true;
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.True);

            // Set up mock data provider.
            DataException testException = new DataException("Test exception attempting to insert new service.");
            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.InsertNewService(It.IsAny<Service>())).Throws(testException);

            // Act.
            this.testNewServiceViewModel.SaveService(new object());

            // Assert.
            // CustomersViewModel should still not contain any customers.
            Assert.That(ServiceViewModelBase.ParentServicesViewModel, Is.Not.Null);
            Assert.That(ServiceViewModelBase.ParentServicesViewModel.Services.Count, Is.EqualTo(0));
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }
    }
}
