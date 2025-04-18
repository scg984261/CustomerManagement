using CDB.Model;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel.ServiceViewModels;
using CustomerManagement.Windows;
using Moq;
using System.Data;

namespace CustomerManagement.Test.ViewModel.ServiceViewModels
{
    public class ServicesViewModelTest
    {
        private NavigationStore testNavigationStore;

        private Mock<IServiceDataProvider> mockServiceDataProvider;
        private IServiceDataProvider mockServiceDataProviderObject;

        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper mockMessageBoxHelperObject;

        private ServicesViewModel testServicesViewModel;

        [SetUp]
        public void Setup()
        {
            this.testNavigationStore = new NavigationStore();

            this.mockServiceDataProvider = new Mock<IServiceDataProvider>();
            this.mockServiceDataProviderObject = this.mockServiceDataProvider.Object;

            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.mockMessageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testServicesViewModel = new ServicesViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.mockMessageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            this.testServicesViewModel = new ServicesViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.mockMessageBoxHelperObject);

            Assert.That(this.testServicesViewModel.Services.Count, Is.EqualTo(0));
            Assert.That(this.testServicesViewModel.ServiceDetailsCommand, Is.Not.Null);
            Assert.That(this.testServicesViewModel.NavigateNewServiceCommand, Is.Not.Null);

            Assert.That(this.testServicesViewModel.SelectedService, Is.Null);
            Assert.That(this.testServicesViewModel.IsServiceSelected(new object()), Is.False);
        }

        [Test]
        public void TestSelectedService()
        {
            this.testServicesViewModel.SelectedService = new ServiceItemViewModel();

            Assert.That(this.testServicesViewModel.SelectedService, Is.Not.Null);
            Assert.That(this.testServicesViewModel.IsServiceSelected(new object()), Is.True);
        }

        [Test]
        public void TestLoad_ShouldNotLoad()
        {
            // Arrange.
            List<Service> testServices = new List<Service>
            {
                new Service("Test servcie 1", 1299.12m, false),
                new Service("Test service 2", 58429.65m, false),
                new Service("Test service 3", 1.89m, true),
                new Service("Test service 4", 2.29m, true),
                new Service("Another Test service", 0.99m, true),
            };

            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.GetAll()).Returns(testServices);

            this.testServicesViewModel.Services.Add(new ServiceItemViewModel());

            // Act.
            this.testServicesViewModel.Load();

            // Assert.
            // This will probably need changed in the future so that Entity Framework re-loads customers each time.
            Assert.That(this.testServicesViewModel.Services.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestLoad_ShouldSuccessfullyLoad()
        {
            // Arrange.
            List<Service> testServices = new List<Service>
            {
                new Service("Test servcie 1", 1299.12m, false),
                new Service("Test service 2", 58429.65m, false),
                new Service("Test service 3", 1.89m, true),
                new Service("Test service 4", 2.29m, true),
                new Service("Another Test service", 0.99m, true),
            };

            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.GetAll()).Returns(testServices);

            // Act.
            this.testServicesViewModel.Load();

            // Assert.
            Assert.That(this.testServicesViewModel.Services.Count, Is.EqualTo(5));
        }

        [Test]
        public void TestLoad_ShouldThrowException()
        {
            // Arrange.
            DataException testDataException = new DataException("Test exception thrown attempting to load services.");
            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.GetAll()).Throws(testDataException);

            // Act.
            DataException expectedException = Assert.Throws<DataException>(() => this.testServicesViewModel.Load());

            // Assert.
            Assert.That(expectedException.Message, Is.EqualTo("Test exception thrown attempting to load services."));
        }

        [Test]
        public void TestNavigateToDetails()
        {
            // Arrange.
            // Select a service.
            this.testServicesViewModel.SelectedService = new ServiceItemViewModel();

            // Act.
            this.testServicesViewModel.NavigateToDetails(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is ServiceDetailsViewModel);
        }

        [Test]
        public void TestIsServiceSelected_ShouldReturnTrue()
        {
            this.testServicesViewModel.SelectedService = new ServiceItemViewModel();

            Assert.That(this.testServicesViewModel.IsServiceSelected(new object()), Is.True);
        }

        [Test]
        public void TestIsSelected_ShouldReturnFalse()
        {
            this.testServicesViewModel.SelectedService = null;

            Assert.That(this.testServicesViewModel.IsServiceSelected(new object()), Is.False);
        }

        [Test]
        public void TestNavigateToNewService()
        {
            // Act.
            this.testServicesViewModel.NavigateToNewService(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is NewServiceViewModel);
        }
    }
}
