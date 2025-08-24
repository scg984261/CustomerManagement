using CDB.Model;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel.ServiceViewModels;
using CustomerManagement.Windows;
using Moq;
using System.Data;

namespace CustomerManagement.Test.ViewModel.ServiceViewModels
{
    public class ServiceDetailsViewModelTest
    {
        private NavigationStore testNavigationStore;

        private Mock<IServiceDataProvider> mockServiceDataProvider;
        private IServiceDataProvider mockServiceDataProviderObject;

        private ServiceDetailsViewModel testServiceDetailsViewModel;

        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper messageBoxHelperObject;

        private ServicesViewModel testServicesViewModel;
        private ServiceItemViewModel testServiceItemViewModel;
        private Service testService;

        [SetUp]
        public void Setup()
        {
            this.testNavigationStore = new NavigationStore();
            this.mockServiceDataProvider = new Mock<IServiceDataProvider>();
            this.mockServiceDataProviderObject = this.mockServiceDataProvider.Object;
            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.messageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testService = new Service
            {
                Id = 1,
                Name = "Test service name.",
                Price = 1.25m,
                IsRecurring = true,
                CreatedDateTime = new DateTime(2025, 04, 12, 19, 01, 27),
                LastUpdateDateTime = new DateTime(2025, 04, 12, 19, 35, 49)
            };
            this.testServiceItemViewModel = new ServiceItemViewModel(this.testService);

            this.testServicesViewModel = new ServicesViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);
            ServiceViewModelBase.ParentServicesViewModel = this.testServicesViewModel;

            this.testServiceDetailsViewModel = new ServiceDetailsViewModel(this.testServiceItemViewModel, this.mockServiceDataProviderObject, this.testNavigationStore, this.messageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            // Re-run the constructor from the setup.
            this.testServiceDetailsViewModel = new ServiceDetailsViewModel(this.testServiceItemViewModel, this.mockServiceDataProviderObject, this.testNavigationStore, this.mockMessageBoxHelper.Object);

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.Id, Is.EqualTo(1));
            Assert.That(this.testServiceDetailsViewModel.Name, Is.EqualTo("Test service name."));
            Assert.That(this.testServiceDetailsViewModel.Price, Is.EqualTo(1.25));
            Assert.That(this.testServiceDetailsViewModel.PriceString, Is.EqualTo("1.25"));
            Assert.That(this.testServiceDetailsViewModel.PriceFormatted, Is.EqualTo("£1.25"));
            Assert.That(this.testServiceDetailsViewModel.IsRecurring, Is.True);
            Assert.That(this.testServiceDetailsViewModel.CreatedDateTime, Is.EqualTo("12-Apr-2025 19:01:27"));
            Assert.That(this.testServiceDetailsViewModel.LastUpdateDateTime, Is.EqualTo("12-Apr-2025 19:35:49"));
        }

        [Test]
        public void TestId()
        {
            // Arrange.
            // Use ServiceDetailsViewModel Created during the Setup.

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.Id, Is.EqualTo(1));
        }


        [Test]
        public void TestCreatedDateTime()
        {
            Assert.That(this.testServiceDetailsViewModel.CreatedDateTime, Is.EqualTo("12-Apr-2025 19:01:27"));
        }

        [Test]
        public void TestLastUpdateDateTime()
        {
            Assert.That(this.testServiceDetailsViewModel.LastUpdateDateTime, Is.EqualTo("12-Apr-2025 19:35:49"));
        }

        [Test]
        public void TestSaveService_ShouldSuccessfullySaveService()
        {
            // Arrange.
            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.UpdateService(1)).Returns(1);

            // Act.
            this.testServiceDetailsViewModel.SaveService(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }

        [Test]
        public void TestSaveService_ShouldCatchException_AndResetValues()
        {
            // Arrange.
            DataException testException = new DataException("Test exception attempting to update service.");
            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.UpdateService(1)).Throws(testException);

            // Set the properties - simulate entering some input.
            this.testServiceDetailsViewModel.Name = "Invalid test service name.";
            this.testServiceDetailsViewModel.PriceString = "2.89";
            this.testServiceDetailsViewModel.IsRecurring = false;

            // Act.
            this.testServiceDetailsViewModel.SaveService(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);

            // Assert that values have been reset to their originals.
            Assert.That(this.testServiceItemViewModel.Name, Is.EqualTo("Test service name."));
            Assert.That(this.testServiceItemViewModel.Price, Is.EqualTo(1.25m));
            Assert.That(this.testServiceItemViewModel.IsRecurring, Is.True);

            Assert.That(this.testServiceDetailsViewModel.Name, Is.EqualTo("Test service name."));
            Assert.That(this.testServiceDetailsViewModel.Price, Is.EqualTo(1.25m));
            Assert.That(this.testServiceDetailsViewModel.IsRecurring, Is.True);
        }

        [Test]
        public void TestNavigateBack()
        {
            // Act.
            this.testServiceDetailsViewModel.NavigateBack(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }

        [Test]
        public void TestCanSaveService_NameIsEmpty_ShouldReturnFalse()
        {
            this.testServiceDetailsViewModel.Name = "";

            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_PriceStringIsEmpty_ShouldReturnFalse()
        {
            this.testServiceDetailsViewModel.PriceString = "";

            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_PriceStringNotValidDecimalValue_ShouldReturnFalse()
        {
            this.testServiceDetailsViewModel.PriceString = "Not a decimal value";

            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_ShouldReturnTrue()
        {
            // Act.
            this.testServiceDetailsViewModel.Name = "Test new service";
            this.testServiceDetailsViewModel.PriceString = "1.95";
            this.testServiceDetailsViewModel.IsRecurring = true;

            // Assert.
            // Should now be able to save the Service Record.
            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.True);
        }
    }
}
