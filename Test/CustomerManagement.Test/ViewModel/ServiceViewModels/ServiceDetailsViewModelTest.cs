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
            ServiceDetailsViewModel.ParentServicesViewModel = this.testServicesViewModel;

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

            Assert.That(this.testServiceDetailsViewModel.SaveCommand, Is.Not.Null);
            Assert.That(this.testServiceDetailsViewModel.CancelCommand, Is.Not.Null);
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
        public void TestName_ShouldTriggerValidation()
        {
            // Arrange/act.
            this.testServiceDetailsViewModel.Name = "";
            IEnumerable<string>? errors = testServiceDetailsViewModel.GetErrors(nameof(testServiceDetailsViewModel.Name)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.Name, Is.EqualTo(string.Empty));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Name of service cannot be blank"));
            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestName_ShouldClearErrors()
        {
            // Arrange/act.
            this.testServiceDetailsViewModel.Name = "Different test service name.";
            IEnumerable<string>? errors = testServiceDetailsViewModel.GetErrors(nameof(testServiceDetailsViewModel.Name)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.Name, Is.EqualTo("Different test service name."));
            Assert.That(errors.Count, Is.EqualTo(0));
            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.True);
        }

        [Test]
        [TestCase(1.89, "1.89", "£1.89")]
        [TestCase(505.272, "505.272", "£505.27")]
        [TestCase(793.816, "793.816", "£793.82")]
        [TestCase(0.82, "0.82", "£0.82")]
        [TestCase(5, "5", "£5.00")]
        public void TestPrice(decimal price, string expectedPriceString, string expectedFormattedPrice)
        {
            // Arrange/Act.
            this.testServiceDetailsViewModel.Price = price;

            Assert.That(this.testServiceDetailsViewModel.Price, Is.EqualTo(price));
            Assert.That(this.testServiceDetailsViewModel.PriceFormatted, Is.EqualTo(expectedFormattedPrice));
        }

        [Test]
        public void TestPriceString_PriceIsEmptyString_ShouldTriggerValidation()
        {
            // Arrange/act.
            this.testServiceDetailsViewModel.PriceString = "";
            IEnumerable<string>? errors = testServiceDetailsViewModel.GetErrors(nameof(testServiceDetailsViewModel.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.PriceString, Is.EqualTo(string.Empty));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Price cannot be blank!"));
            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestPriceString_PriceIsNotValidDecimal_ShouldTriggerValidation()
        {
            // Arrange/Act.
            this.testServiceDetailsViewModel.PriceString = "Not valid decimal value.";
            IEnumerable<string>? errors = testServiceDetailsViewModel.GetErrors(nameof(testServiceDetailsViewModel.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.PriceString, Is.EqualTo("Not valid decimal value."));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Value must be a valid decimal."));
            Assert.That(this.testServiceDetailsViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        [TestCase("0.5", 0.5, "£0.50")]
        [TestCase("2842587.38429", 2842587.38429, "£2842587.38")]
        [TestCase("1.889", 1.889, "£1.89")]
        [TestCase("55", 55.0, "£55.00")]
        [TestCase(".22", 0.22, "£0.22")]
        public void TestPriceString_ShouldClearErrors(string inputPriceString, decimal expectedPrice, string expectedPriceFormatted)
        {
            // Arrange/Act.
            this.testServiceDetailsViewModel.PriceString = inputPriceString;
            IEnumerable<string>? errors = testServiceDetailsViewModel.GetErrors(nameof(testServiceDetailsViewModel.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.PriceString, Is.EqualTo(inputPriceString));
            Assert.That(this.testServiceDetailsViewModel.Price, Is.EqualTo(expectedPrice));
            Assert.That(this.testServiceDetailsViewModel.PriceFormatted, Is.EqualTo(expectedPriceFormatted));
        }

        [Test]
        public void TestPriceFormatted()
        {
            // Arrange/Act.
            // Set the price to some valid value.
            this.testServiceDetailsViewModel.PriceString = ".89";

            Assert.That(this.testServiceDetailsViewModel.PriceFormatted, Is.EqualTo("£0.89"));
        }

        [Test]
        public void TestIsRecurring_ShouldReturnTrue()
        {
            this.testServiceDetailsViewModel.IsRecurring = true;

            Assert.That(this.testServiceDetailsViewModel.IsRecurring, Is.True);
        }

        [Test]
        public void TestIsRecurring_ShouldReturnFalse()
        {
            this.testServiceDetailsViewModel.IsRecurring = false;

            Assert.That(this.testServiceDetailsViewModel.IsRecurring, Is.False);
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
        public void TestSaveService_ShouldCatchException()
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
            Assert.That(this.testServiceDetailsViewModel.Name, Is.EqualTo("Test service name."));
            Assert.That(this.testServiceDetailsViewModel.Price, Is.EqualTo(1.25m));
            Assert.That(this.testServiceDetailsViewModel.IsRecurring, Is.True);
        }

        [Test]
        public void TestNavigateBack()
        {
            // Act.
            this.testServiceDetailsViewModel.NavigateBack();

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

        [Test]
        public void TestCancel()
        {
            // Arrange.
            // Set the properties - simulate entering some input.
            this.testServiceDetailsViewModel.Name = "Different name - should be reset after cancellation.";
            this.testServiceDetailsViewModel.PriceString = "2.29";
            this.testServiceDetailsViewModel.IsRecurring = false;

            // Act.
            this.testServiceDetailsViewModel.Cancel(new object());

            // Assert.
            Assert.That(this.testServiceDetailsViewModel.Name, Is.EqualTo("Test service name."));
            Assert.That(this.testServiceDetailsViewModel.Price, Is.EqualTo(1.25m));
            Assert.That(this.testServiceDetailsViewModel.IsRecurring, Is.True);
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }
    }
}
